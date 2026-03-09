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

    [Header("Sprites")]
    public Sprite redSprite;
    public Sprite blueSprite;

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
        CreateDegreeLabel();
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

        spriteRenderer.color = Color.white;
        if (starColor == StarColor.Red && redSprite != null)
            spriteRenderer.sprite = redSprite;
        else if (starColor == StarColor.Blue && blueSprite != null)
            spriteRenderer.sprite = blueSprite;
    }

    private void CreateDegreeLabel()
    {
        GameObject labelObj = new GameObject("DegreeLabel");
        labelObj.transform.SetParent(transform, false);
        labelObj.transform.localPosition = new Vector3(0, 0, -1);

        TextMesh textMesh = labelObj.AddComponent<TextMesh>();
        textMesh.text = maxDegree.ToString();
        textMesh.color = Color.black;
        textMesh.fontSize = 160 ;
        textMesh.characterSize = 0.5f;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Center;

        // Render above the star sprite
        MeshRenderer mr = labelObj.GetComponent<MeshRenderer>();
        mr.sortingOrder = spriteRenderer != null ? spriteRenderer.sortingOrder + 1 : 1;
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