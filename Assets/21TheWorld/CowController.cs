using UnityEngine;

// drives a single cow's behavior: idles until the player gets close, then
// joins the herd by holding a slot in a ring around the player. when the
// player charges toward it, the cow scrambles out of the way to avoid being
// run over and to keep the herd visually loose instead of clumped.
public class CowController : MonoBehaviour
{
    // distance at which an idle cow notices the player and starts following.
    // must be larger than (cow collider radius + player collider radius) or the
    // player's collider will physically push the cow away before this fires.
    [SerializeField] private float triggerRadius = 2.5f;
    // base movement speed when chasing the assigned slot in the herd.
    [SerializeField] private float followSpeed = 4f;
    // desired radius from the player when the player is moving.
    [SerializeField] private float followDistance = 1.8f;
    // extra slack added when the player stops, so cows don't crowd a stationary player.
    [SerializeField] private float stationaryFollowBuffer = 0.35f;
    // player speed (squared via threshold * threshold) below which we treat the player as stopped.
    [SerializeField] private float stationarySpeedThreshold = 0.05f;
    // minimum dot product between the player's velocity and the cow direction
    // before the cow considers itself "in the way" and backs up. zero means
    // "anything not directly behind the player counts as in front of motion".
    [SerializeField] private float approachDotThreshold = 0f;
    // how far past the normal follow distance to retreat when dodging the player.
    [SerializeField] private float backupDistance = 1f;
    // speed multiplier while backing up — moves faster than normal follow so it can clear out.
    [SerializeField] private float backupSpeedMultiplier = 1.3f;
    // extra range beyond followDistance where the back-up reaction can still trigger.
    [SerializeField] private float backupTriggerBuffer = 0.6f;
    // half-width of the corridor (perpendicular to player motion) inside which a
    // cow ahead of the player is considered "in the swept path" and yields.
    [SerializeField] private float backupCorridorHalfWidth = 0.8f;
    // distance at which a following cow gives up and returns to idle. must be
    // larger than triggerRadius so a cow on the edge of recruitment range doesn't
    // flicker between idle and following each frame.
    [SerializeField] private float maxFollowDistance = 5f;
    // per-cow absolute angle (radians) on the herd ring around the player,
    // measured from world +X. assigned by the spawner so the herd fans out evenly.
    [HideInInspector] public float angularOffset = 0f;

    private Transform playerTransform;
    private Rigidbody2D playerRb;
    private bool isFollowing;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // called by the spawner to bind this cow to a specific player. caches the
    // rigidbody2d so we can read velocity each fixedupdate without re-fetching.
    public void Init(Transform player)
    {
        playerTransform = player;
        playerRb = playerTransform != null ? playerTransform.GetComponent<Rigidbody2D>() : null;
    }

    // win condition checks this — every cow being in the follow state means the player has rounded up the herd.
    public bool IsFollowing()
    {
        return isFollowing;
    }

    void Update()
    {
        if (playerTransform == null)
        {
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (isFollowing)
        {
            // dropped too far behind — give up and idle. zero velocity so the cow
            // doesn't coast once FixedUpdate stops driving it.
            if (distanceToPlayer > maxFollowDistance)
            {
                isFollowing = false;
                if (rb != null)
                {
                    rb.linearVelocity = Vector2.zero;
                }
            }
            return;
        }

        // proximity-based recruitment: walking near a cow adds it to the herd.
        if (distanceToPlayer <= triggerRadius)
        {
            isFollowing = true;
        }
    }

    void FixedUpdate()
    {
        if (!isFollowing || playerTransform == null || rb == null)
        {
            return;
        }

        // angularOffset is the cow's absolute slot angle on the ring (world +X = 0).
        // computing the slot direction directly — instead of rotating the cow's
        // current direction-from-player — gives every cow a stable, fixed target
        // so the herd settles into a ring instead of spiraling around the player.
        Vector2 slotDir = new Vector2(Mathf.Cos(angularOffset), Mathf.Sin(angularOffset));

        // when the player stops, widen the follow ring so cows settle a bit further out instead of bumping into the player.
        bool playerIsStationary = playerRb == null || playerRb.linearVelocity.sqrMagnitude <= stationarySpeedThreshold * stationarySpeedThreshold;
        float targetFollowDistance = playerIsStationary ? followDistance + stationaryFollowBuffer : followDistance;

        Vector2 playerPos = playerTransform.position;
        Vector2 targetPos = playerPos + slotDir * targetFollowDistance;

        // if the player is heading toward (or past) this cow, override the slot
        // target and aim for a position further along the direct away vector.
        bool shouldBackUp = ShouldBackUp(playerPos);
        if (shouldBackUp)
        {
            Vector2 awayFromPlayer = rb.position - playerPos;
            if (awayFromPlayer.sqrMagnitude < 0.0001f)
            {
                awayFromPlayer = slotDir;
            }
            else
            {
                awayFromPlayer.Normalize();
            }

            targetPos = playerPos + awayFromPlayer * (targetFollowDistance + backupDistance);
        }

        Vector2 toTarget = targetPos - rb.position;
        float distToTarget = toTarget.magnitude;
        float moveSpeed = followSpeed * (shouldBackUp ? backupSpeedMultiplier : 1f);

        // small deadband prevents jitter when the cow is essentially in position.
        // clamp velocity so we never overshoot the slot in a single physics step,
        // which was a secondary source of ringing/jitter at slot boundaries.
        if (distToTarget > 0.05f)
        {
            float stepLimit = distToTarget / Mathf.Max(Time.fixedDeltaTime, 0.0001f);
            rb.linearVelocity = (toTarget / distToTarget) * Mathf.Min(moveSpeed, stepLimit);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    // returns true when the player is moving fast enough, is close enough,
    // and is heading toward (or past) this cow — i.e. about to collide with it
    // OR about to brush past it in a way that would otherwise leave the cow
    // standing in the player's path.
    bool ShouldBackUp(Vector2 playerPos)
    {
        if (!isFollowing || playerRb == null)
        {
            return false;
        }

        // player must actually be moving for this to be a real "approach".
        Vector2 playerVelocity = playerRb.linearVelocity;
        float playerSpeedSq = playerVelocity.sqrMagnitude;
        float minSpeedSq = stationarySpeedThreshold * stationarySpeedThreshold;
        if (playerSpeedSq <= minSpeedSq)
        {
            return false;
        }

        Vector2 playerToCow = rb.position - playerPos;
        if (playerToCow.sqrMagnitude < 0.0001f)
        {
            // already overlapping — get out immediately.
            return true;
        }

        // only react when the player is roughly within follow range; ignore distant approaches.
        float currentDistance = playerToCow.magnitude;
        float backupTriggerDistance = followDistance + backupTriggerBuffer;
        if (currentDistance > backupTriggerDistance)
        {
            return false;
        }

        Vector2 playerVelDir = playerVelocity.normalized;
        Vector2 playerToCowDir = playerToCow / currentDistance;

        // direct head-on: player velocity points roughly toward this cow.
        float approachingDot = Vector2.Dot(playerVelDir, playerToCowDir);
        if (approachingDot > approachDotThreshold)
        {
            return true;
        }

        // corridor check: cow is ahead of the player along motion (parallel > 0)
        // and within a narrow swept lane (perpendicular distance small). this
        // covers the "flanking cow blocking the path" case the head-on dot misses.
        float parallel = Vector2.Dot(playerToCow, playerVelDir);
        if (parallel <= 0f)
        {
            return false;
        }

        Vector2 perpendicular = playerToCow - playerVelDir * parallel;
        return perpendicular.magnitude <= backupCorridorHalfWidth;
    }
}
