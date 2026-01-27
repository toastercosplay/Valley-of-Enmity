using UnityEngine;

public class Scarecrow : MonoBehaviour
{
    float maxDepth = 75f;
    float minDepth = -200f;

    float maxScale = 1.15f;
    float minScale = .5f;
    float maxX = 425f;
    float minX = -400f;

    // [SerializeField] float testDepth;
    // [SerializeField] float testScale;
    // [SerializeField] float testX;
    // [SerializeField] int testSiblingIndex;

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

        if (depth > -25f)
        {
            imageRectTransform.SetSiblingIndex(1);
        }
        else if (scale > -125f)
        {
            imageRectTransform.SetSiblingIndex(2);
        }
        else if (scale > -175f)
        {
            imageRectTransform.SetSiblingIndex(3);
        }
        else
        {
            imageRectTransform.SetSiblingIndex(4);
        }
    }

    void Update()
    {
        //for testing
        // imageRectTransform.anchoredPosition = new Vector2(testX, testDepth);
        // imageRectTransform.localScale = new Vector3(testScale, testScale, 1f);
        // imageRectTransform.SetSiblingIndex(testSiblingIndex);

    }
}
