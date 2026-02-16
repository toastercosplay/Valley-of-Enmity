using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using TMPro;

public class ScoringText : MonoBehaviour
{
    public static ScoringText instance;
    [SerializeField] private TextMeshProUGUI currentScoreText;
    private int score;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentScoreText.text = score.ToString();        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void UpdateScore()
    {
        score++;
        currentScoreText.text = score.ToString();
    }
}
