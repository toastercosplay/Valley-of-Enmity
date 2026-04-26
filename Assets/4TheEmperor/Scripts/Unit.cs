using UnityEngine;
using UnityEngine.Splines;

public class Unit : MonoBehaviour
{
    private SplineContainer targetSpline;
    private float splineLength;
    private float distanceTraveled = 0f;
    
    private float minSize, maxSize, growthSpeed;
    private float minSpeed, maxSpeed;
    
    private float currentScale;
    private bool isCharging = false;
    private bool isReleased = false;

    private int currentHP;

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
        
        //start of path snapping
        transform.position = (Vector3)targetSpline.EvaluatePosition(0);
        
        isCharging = true;
        currentHP = 10;
    }

    public void Release()
    {
        isCharging = false;
        isReleased = true;

        // bigger and slower
        float t = Mathf.InverseLerp(minSize, maxSize, currentScale);
        float finalSpeed = Mathf.Lerp(maxSpeed, minSpeed, t);

        currentHP = Mathf.RoundToInt(Mathf.Lerp(1, 8, t));
        
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

        distanceTraveled += growthSpeed * Time.deltaTime;
        float t = Mathf.Clamp01(distanceTraveled / splineLength);

        Vector3 pos = targetSpline.EvaluatePosition(t);
        transform.position = new Vector3(pos.x, pos.y, 0f);

        Vector3 tangent = targetSpline.EvaluateTangent(t);
        if (tangent != Vector3.zero)
        {
            float angle = Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        if (t >= 1f) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            TakeDamage(1);
            
            // Destroy(other.gameObject); 
        }
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        if (currentHP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
