using UnityEngine;

// drives a single cow's behavior: idles until the player gets close, then
// joins the herd by holding a slot in a ring around the player. when the
// player charges toward it, the cow scrambles out of the way to avoid being
// run over and to keep the herd visually loose instead of clumped.
public class CowController : MonoBehaviour
{
    // distance at which an idle cow notices the player and starts following.
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
    // before the cow considers itself "in the way" and backs up.
    [SerializeField] private float approachDotThreshold = 0.25f;
    // how far past the normal follow distance to retreat when dodging the player.
    [SerializeField] private float backupDistance = 1f;
    // speed multiplier while backing up — moves faster than normal follow so it can clear out.
    [SerializeField] private float backupSpeedMultiplier = 1.3f;
    // extra range beyond followDistance where the back-up reaction can still trigger.
    [SerializeField] private float backupTriggerBuffer = 0.6f;
    // per-cow angle (radians) around the player; assigned by the spawner so the herd fans out evenly.
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
        // once recruited, the cow stays recruited; nothing to check.
        if (isFollowing || playerTransform == null)
        {
            return;
        }

        // proximity-based recruitment: walking near a cow adds it to the herd.
        if (Vector2.Distance(transform.position, playerTransform.position) <= triggerRadius)
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

        // direction from the player to this cow's current position. this is
        // the baseline angle we'll rotate by angularoffset to get our slot.
        Vector2 dirFromPlayer = (Vector2)(transform.position - playerTransform.position);
        if (dirFromPlayer.sqrMagnitude < 0.0001f)
        {
            // avoid normalizing a zero vector when the cow is sitting on top of the player.
            dirFromPlayer = Vector2.right;
        }
        else
        {
            dirFromPlayer.Normalize();
        }

        // rotate dirfromplayer by angularoffset (2d rotation matrix) so each
        // cow targets a different angle around the player and the herd spreads out.
        float cos = Mathf.Cos(angularOffset);
        float sin = Mathf.Sin(angularOffset);
        Vector2 rotatedDir = new Vector2(
            dirFromPlayer.x * cos - dirFromPlayer.y * sin,
            dirFromPlayer.x * sin + dirFromPlayer.y * cos
        );

        // when the player stops, widen the follow ring so cows settle a bit further out instead of bumping into the player.
        bool playerIsStationary = playerRb == null || playerRb.linearVelocity.sqrMagnitude <= stationarySpeedThreshold * stationarySpeedThreshold;
        float targetFollowDistance = playerIsStationary ? followDistance + stationaryFollowBuffer : followDistance;

        Vector2 playerPos = playerTransform.position;
        Vector2 targetPos = playerPos + rotatedDir * targetFollowDistance;

        // if the player is barreling toward this cow, override the slot target
        // and instead aim for a position further along the direct away vector.
        bool shouldBackUp = ShouldBackUp(playerPos);
        if (shouldBackUp)
        {
            Vector2 awayFromPlayer = rb.position - playerPos;
            if (awayFromPlayer.sqrMagnitude < 0.0001f)
            {
                awayFromPlayer = rotatedDir;
            }
            else
            {
                awayFromPlayer.Normalize();
            }

            targetPos = playerPos + awayFromPlayer * (targetFollowDistance + backupDistance);
        }

        Vector2 toTarget = targetPos - rb.position;
        float moveSpeed = followSpeed * (shouldBackUp ? backupSpeedMultiplier : 1f);

        // small deadband prevents jitter when the cow is essentially in position.
        if (toTarget.magnitude > 0.05f)
        {
            rb.linearVelocity = toTarget.normalized * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    // returns true when the player is moving fast enough, is close enough,
    // and is heading toward this cow — i.e. about to collide with it.
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

        // dot product > threshold means the player's velocity points toward this cow.
        float approachingDot = Vector2.Dot(playerVelocity.normalized, playerToCow.normalized);
        return approachingDot > approachDotThreshold;
    }
}
