using UnityEngine;
using UnityEngine.Splines;

public class Unit : MonoBehaviour
{
    private SplineContainer targetSpline;
    private float splineLength;
    private float distanceTraveled = 0f;
    
    // Growth Settings (Passed from the Attacker)
    private float minSize, maxSize, growthSpeed;
    private float minSpeed, maxSpeed;
    
    private float currentScale;
    private bool isCharging = false;
    private bool isReleased = false;

    private int currentHP;


    // We pass all the "rules" of this unit here
    public void StartCharging(SplineContainer spline, float minS, float maxS, float growS, float minV, float maxV)
    {
        targetSpline = spline;
        splineLength = targetSpline.CalculateLength();
        
        minSize = minS;
        maxSize = maxS;
        growthSpeed = growS;
        minSpeed = minV;
        maxSpeed = maxV;

        currentScale = minSize;
        transform.localScale = Vector3.one * currentScale;
        
        // Snap to start of path
        transform.position = (Vector3)targetSpline.EvaluatePosition(0);
        
        isCharging = true;
        currentHP = 1;
    }

    public void Release()
    {
        isCharging = false;
        isReleased = true;

        // Calculate speed based on final scale
        // Bigger = Slower
        float t = Mathf.InverseLerp(minSize, maxSize, currentScale);
        float finalSpeed = Mathf.Lerp(maxSpeed, minSpeed, t);

        currentHP = Mathf.RoundToInt(Mathf.Lerp(1, 5, t));
        
        // Store speed for movement logic
        growthSpeed = finalSpeed; 
        
    }

    void Update()
    {
        if (isCharging)
        {
            currentScale = Mathf.MoveTowards(currentScale, maxSize, growthSpeed * Time.deltaTime);
            if (currentScale >= maxSize)
            {
                currentScale = maxSize;
                Release();
            }
            transform.localScale = Vector3.one * currentScale;
        }
        else if (isReleased)
        {
            MoveAlongPath();
        }
    }

    void MoveAlongPath()
    {
        if (targetSpline == null) return;

        distanceTraveled += growthSpeed * Time.deltaTime; // Using growthSpeed variable as actual speed now
        float t = Mathf.Clamp01(distanceTraveled / splineLength);

        // 2D Position (Forcing Z to 0)
        Vector3 pos = targetSpline.EvaluatePosition(t);
        transform.position = new Vector3(pos.x, pos.y, 0f);

        // 2D Rotation
        Vector3 tangent = targetSpline.EvaluateTangent(t);
        if (tangent != Vector3.zero)
        {
            float angle = Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        if (t >= 1f) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other) // Changed to Enter so it doesn't take 60dmg/sec
    {
        if (other.CompareTag("Projectile"))
        {
            TakeDamage(1);
            
            // Usually you'd want to destroy the projectile here too:
            // Destroy(other.gameObject); 
        }
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        if (currentHP <= 0)
        {
            // Add death effects here if you want!
            Destroy(gameObject);
        }
    }
}
