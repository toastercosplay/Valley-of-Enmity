using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TheDevil : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] float totalTimeLimit = 15f;
    [SerializeField] float countdownTime = 3f;
    [SerializeField] float outOfOrderPenalty = 2f;

    [Header("References")]
    [SerializeField] DevilPlayer[] players = new DevilPlayer[4];

    [Header("UI")]
    [SerializeField] TMP_Text[] playerTimeTexts = new TMP_Text[4];
    [SerializeField] TMP_Text countdownText;

    GameManager gameManager;

    int playerCount;
    int currentPlayerTurn; // 0-(playerCount-1), -1 = waiting to start
    bool gameStarted;
    bool gameEnded;
    float countdownTimer;

    float[] playerTimes;

    void Start()
    {
        gameManager = GameManager.Instance;
        countdownTimer = countdownTime;
        currentPlayerTurn = -1;
        gameStarted = false;
        gameEnded = false;

        playerCount = GetPlayerCount();
        playerTimes = new float[playerCount];
    }

    int GetPlayerCount()
    {
        if (GameManager.Instance != null)
        {
            int count = 2;
            if (GameManager.Instance.player3 != null && GameManager.Instance.player3.gameObject.activeSelf)
                count = 3;
            if (GameManager.Instance.player4 != null && GameManager.Instance.player4.gameObject.activeSelf)
                count = 4;
            return count;
        }
        return 2;
    }

    void Update()
    {
        if (gameEnded) return;

        if (!gameStarted)
        {
            countdownTimer -= Time.deltaTime;
            if (countdownText != null)
                countdownText.text = Mathf.CeilToInt(countdownTimer).ToString();
            if (countdownTimer <= 0)
            {
                countdownText.text = "";
                StartGame();
            }
            return;
        }

        if (currentPlayerTurn >= 0)
            playerTimes[currentPlayerTurn] += Time.deltaTime;

        float totalTime = 0f;
        for (int i = 0; i < playerCount; i++)
            totalTime += playerTimes[i];

        if (totalTime >= totalTimeLimit)
        {
            EndGame();
            return;
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        for (int i = 0; i < playerCount; i++)
            if (playerTimeTexts[i] != null)
                playerTimeTexts[i].text = FormatTime(playerTimes[i]);
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time * 100f) % 100f);
        return $"{minutes:00}:{seconds:00}.{milliseconds:00}";
    }

    void StartGame()
    {
        gameStarted = true;
        currentPlayerTurn = 0;

        for (int i = 0; i < playerCount; i++)
            players[i].SetActive(i == 0);
    }

    public void OnPlayerClick(DevilPlayer player)
    {
        if (!gameStarted || gameEnded) return;

        int playerNumber = System.Array.IndexOf(players, player);

        if (playerNumber == currentPlayerTurn)
        {
            ApplyPenalty(playerNumber);
            return;
        }

        SwitchTurn(playerNumber);
        
    }

    void SwitchTurn(int playerNumber)
    {
        players[currentPlayerTurn].SetActive(false);
        currentPlayerTurn = playerNumber;
        players[currentPlayerTurn].SetActive(true);
    }

    void ApplyPenalty(int playerIndex)
    {
        playerTimes[playerIndex] = Mathf.Max(0f, playerTimes[playerIndex] - outOfOrderPenalty);
        players[playerIndex].OnPenalty();
    }

    void EndGame()
    {
        gameEnded = true;

        for (int i = 0; i < playerCount; i++)
            players[i].SetActive(false);

        int winner = 0;
        for (int i = 1; i < playerCount; i++)
            if (playerTimes[i] > playerTimes[winner])
                winner = i;

        for (int i = 0; i < playerCount; i++)
            players[i].SetResult(i == winner ? 1 : 3);

        Invoke(nameof(FinishGame), 2f);
    }

    void FinishGame()
    {
        gameManager.FinishMinigame();
    }

    public bool IsGameActive()
    {
        return gameStarted && !gameEnded;
    }
}
