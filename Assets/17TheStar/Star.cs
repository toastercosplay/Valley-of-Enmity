using UnityEngine;

public enum StarColor
{
    Red,
    Blue
}

public enum BipartiteSet
{
    A,
    B
}

public class Star : MonoBehaviour
{
    [Header("Settings")]
    public StarColor starColor;
    public BipartiteSet bipartiteSet;

    [Header("Sprites")]
    public Sprite redSprite;
    public Sprite blueSprite;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        // Randomize starting color
        starColor = Random.value > 0.5f ? StarColor.Red : StarColor.Blue;
        UpdateColorVisuals();
    }

    void OnValidate()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateColorVisuals();
    }

    public void ToggleColor()
    {
        starColor = starColor == StarColor.Red ? StarColor.Blue : StarColor.Red;
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
}
