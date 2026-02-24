using UnityEngine;
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

    // Helper function to check if this star can accept a new connection
    public bool CanConnect()
    {
        return connectedStars.Count < maxDegree;
    }
}