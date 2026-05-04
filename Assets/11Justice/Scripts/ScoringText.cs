using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using TMPro;

public class ScoringText : MonoBehaviour
{
    public static ScoringText instance;
    [SerializeField] private TextMeshProUGUI player1ScoreText;
    [SerializeField] private TextMeshProUGUI player2ScoreText;
    [SerializeField] private TextMeshProUGUI player3ScoreText;
    [SerializeField] private TextMeshProUGUI player4ScoreText;
    private int[] scores = new int[4];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player1ScoreText.text = scores[0].ToString();
        player2ScoreText.text = scores[1].ToString();
        player3ScoreText.text = scores[2].ToString();
        player4ScoreText.text = scores[3].ToString();
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

    public void UpdateScore(int playerIndex)
    {
        scores[playerIndex]++;
        
        if (playerIndex == 0)
        {
            player1ScoreText.text = scores[0].ToString();
        }
        else if (playerIndex == 1)
        {
            player2ScoreText.text = scores[1].ToString();
        }
        else if (playerIndex == 2)
        {
            player3ScoreText.text = scores[2].ToString();
        }
        else if (playerIndex == 3)
        {
            player4ScoreText.text = scores[3].ToString();
        }
    }
}
