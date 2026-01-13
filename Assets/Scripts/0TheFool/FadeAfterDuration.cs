using UnityEngine;
using UnityEngine.UI;

public class FadeAfterDuration : MonoBehaviour
{
    [SerializeField] public float duration = 1f;
    [SerializeField] public float elapsed = -1f;

    Image image;

    bool currentlyEnabled = true;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / duration);

        Color c = image.color;
        c.a = Mathf.Lerp(1f, 0f, t);
        if (currentlyEnabled)
        {
            image.color = c;
        }
    }

    public void Disable()
    {
        currentlyEnabled = false;
    }
}
