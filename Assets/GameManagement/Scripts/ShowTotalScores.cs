using UnityEngine;
using TMPro;

public class ShowTotalScores : MonoBehaviour
{
    PlayerData player1Data;
    PlayerData player2Data;

    [SerializeField] TextMeshProUGUI player1ScoreText;
    [SerializeField] TextMeshProUGUI player2ScoreText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player2Data = GameObject.FindGameObjectWithTag("Player2Data").GetComponent<PlayerData>();
        player1Data = GameObject.FindGameObjectWithTag("Player1Data").GetComponent<PlayerData>();
    }

    // Update is called once per frame
    void Update()
    {
        player1ScoreText.text = player1Data.GetTotalScore().ToString();
        player2ScoreText.text = player2Data.GetTotalScore().ToString();
    }
}
