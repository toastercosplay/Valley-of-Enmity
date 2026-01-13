using UnityEngine;

public class Scarecrow : MonoBehaviour
{
    float maxDepth = 400f;
    float minDepth = 200f;

    float maxScale = 1.25f;
    float minScale = .5f;

    float maxX = 900f;
    float minX = 40f;

    private RectTransform imageRectTransform;
    
    void Start()
    {
        imageRectTransform = GetComponent<RectTransform>();

        float seed = Random.Range(0f, 1f);

        float depth = Mathf.Lerp(minDepth, maxDepth, seed);
        float scale = Mathf.Lerp(maxScale, minScale, seed);

        imageRectTransform.anchoredPosition = new Vector2(Random.Range(minX, maxX), depth);
        imageRectTransform.localScale = new Vector3(scale, scale, 1f);

        //Debug.Log(seed);
    }
}
