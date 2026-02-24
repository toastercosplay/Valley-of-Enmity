using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum StarColor
{
    Red,
    Blue
}

public class Star : MonoBehaviour
{
    [Header("Settings")]
    public StarColor starColor;     
    public int maxDegree = 3;       

    [Header("State")]
    // We keep a list of connected stars to prevent connecting the same two stars twice
    public List<Star> connectedStars = new List<Star>();

    // Visuals
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        UpdateColorVisuals();
    }

    // Call this whenever you change the variable in the inspector to see changes live
    void OnValidate()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateColorVisuals();
    }

    void UpdateColorVisuals()
    {
        if (spriteRenderer == null) return;

        // Simple logic to change sprite color based on the enum
        if (starColor == StarColor.Red)
            spriteRenderer.color = Color.red;
        else
            spriteRenderer.color = Color.blue;
    }

    [Header("Win")]
    public Vector2 winDestination;

    public bool IsSatisfied => connectedStars.Count == maxDegree;

    public bool CanConnect()
    {
        return connectedStars.Count < maxDegree;
    }

    public void MoveToWinDestination(float duration)
    {
        StartCoroutine(LerpToPosition(winDestination, duration));
    }

    private IEnumerator LerpToPosition(Vector2 target, float duration)
    {
        Vector3 start = transform.position;
        Vector3 end = new Vector3(target.x, target.y, start.z);
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(start, end, elapsed / duration);
            yield return null;
        }
        transform.position = end;
    }
}