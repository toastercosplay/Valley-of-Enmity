using UnityEngine;

public class CowController : MonoBehaviour
{
    [SerializeField] private float triggerRadius = 2.5f;
    [SerializeField] private float followSpeed = 4f;
    [SerializeField] private float followDistance = 1.8f;
    [SerializeField] private float stationaryFollowBuffer = 0.35f;
    [SerializeField] private float stationarySpeedThreshold = 0.05f;
    [SerializeField] private float approachDotThreshold = 0.25f;
    [SerializeField] private float backupDistance = 1f;
    [SerializeField] private float backupSpeedMultiplier = 1.3f;
    [SerializeField] private float backupTriggerBuffer = 0.6f;
    [HideInInspector] public float angularOffset = 0f;

    private Transform playerTransform;
    private Rigidbody2D playerRb;
    private bool isFollowing;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(Transform player)
    {
        playerTransform = player;
        playerRb = playerTransform != null ? playerTransform.GetComponent<Rigidbody2D>() : null;
    }

    public bool IsFollowing()
    {
        return isFollowing;
    }

    void Update()
    {
        if (isFollowing || playerTransform == null)
        {
            return;
        }

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

        Vector2 dirFromPlayer = (Vector2)(transform.position - playerTransform.position);
        if (dirFromPlayer.sqrMagnitude < 0.0001f)
        {
            dirFromPlayer = Vector2.right;
        }
        else
        {
            dirFromPlayer.Normalize();
        }

        float cos = Mathf.Cos(angularOffset);
        float sin = Mathf.Sin(angularOffset);
        Vector2 rotatedDir = new Vector2(
            dirFromPlayer.x * cos - dirFromPlayer.y * sin,
            dirFromPlayer.x * sin + dirFromPlayer.y * cos
        );

        bool playerIsStationary = playerRb == null || playerRb.linearVelocity.sqrMagnitude <= stationarySpeedThreshold * stationarySpeedThreshold;
        float targetFollowDistance = playerIsStationary ? followDistance + stationaryFollowBuffer : followDistance;

        Vector2 playerPos = playerTransform.position;
        Vector2 targetPos = playerPos + rotatedDir * targetFollowDistance;

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

        if (toTarget.magnitude > 0.05f)
        {
            rb.linearVelocity = toTarget.normalized * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    bool ShouldBackUp(Vector2 playerPos)
    {
        if (!isFollowing || playerRb == null)
        {
            return false;
        }

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
            return true;
        }

        float currentDistance = playerToCow.magnitude;
        float backupTriggerDistance = followDistance + backupTriggerBuffer;
        if (currentDistance > backupTriggerDistance)
        {
            return false;
        }

        float approachingDot = Vector2.Dot(playerVelocity.normalized, playerToCow.normalized);
        return approachingDot > approachDotThreshold;
    }
}
