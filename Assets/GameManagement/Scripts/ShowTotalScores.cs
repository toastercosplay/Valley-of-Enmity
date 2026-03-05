using UnityEngine;
using TMPro;

public class ShowTotalScores : MonoBehaviour
{
    PlayerData player1Data;
    PlayerData player2Data;
    PlayerData player3Data;
    PlayerData player4Data;

    [SerializeField] TextMeshProUGUI player1ScoreText;
    [SerializeField] TextMeshProUGUI player2ScoreText;
    [SerializeField] TextMeshProUGUI player3ScoreText;
    [SerializeField] TextMeshProUGUI player4ScoreText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player2Data = GameObject.FindGameObjectWithTag("Player2Data").GetComponent<PlayerData>();
        player1Data = GameObject.FindGameObjectWithTag("Player1Data").GetComponent<PlayerData>();
        player3Data = GameObject.FindGameObjectWithTag("Player3Data").GetComponent<PlayerData>();
        player4Data = GameObject.FindGameObjectWithTag("Player4Data").GetComponent<PlayerData>();
    }

    // Update is called once per frame
    void Update()
    {
        player1ScoreText.text = player1Data.GetTotalScore().ToString();
        player2ScoreText.text = player2Data.GetTotalScore().ToString();
        if (player3Data!= null)
        {
            player3ScoreText.text = player3Data.GetTotalScore().ToString();
        }
        if (player4Data != null)
        {
            player4ScoreText.text = player4Data.GetTotalScore().ToString();
        }
    }
}
