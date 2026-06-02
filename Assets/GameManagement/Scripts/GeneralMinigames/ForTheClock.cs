using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ForTheClock : MonoBehaviour
{
    [SerializeField] private UnityEvent onTimeEnd;

    [SerializeField] private float totalTime = 30f;
    private float currentTime;

    [SerializeField] private Sprite[] frames = new Sprite[8];

    private Image renderer;
    private bool timeEnded = false;

    void Start()
    {
        renderer = GetComponent<Image>();

        currentTime = totalTime;

        if (frames.Length > 0)
        {
            renderer.sprite = frames[0];
        }
    }

    void Update()
    {
        if (timeEnded)
            return;

        currentTime -= Time.deltaTime;
        currentTime = Mathf.Max(currentTime, 0);

        float percentRemaining = currentTime / totalTime;

        int frameIndex = Mathf.Clamp(
            Mathf.FloorToInt((1f - percentRemaining) * frames.Length),
            0,
            frames.Length - 1
        );

        if (frames.Length > 0)
        {
            renderer.sprite = frames[frameIndex];
        }

        if (currentTime <= 0)
        {
            timeEnded = true;
            onTimeEnd?.Invoke();
        }
    }

    public void ResetTimer()
    {
        currentTime = totalTime;
        timeEnded = false;

        if (frames.Length > 0)
        {
            renderer.sprite = frames[0];
        }
    }
}