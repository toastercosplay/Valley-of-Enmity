using UnityEngine;
using TMPro;

public class TheDevil : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] float totalTimeLimit = 30f;
    [SerializeField] float countdownTime = 3f;
    [SerializeField] float outOfOrderPenalty = 2f;

    [Header("References")]
    [SerializeField] DevilPlayer playerOne;
    [SerializeField] DevilPlayer playerTwo;

    [Header("UI")]
    [SerializeField] TMP_Text playerOneTimeText;
    [SerializeField] TMP_Text playerTwoTimeText;
    [SerializeField] TMP_Text countdownText;

    GameManager gameManager;

    int currentPlayerTurn; // 0 = waiting to start, 1 = player one's turn, 2 = player two's turn
    bool gameStarted;
    bool gameEnded;
    float countdownTimer;

    float playerOneTime;
    float playerTwoTime;

    void Start()
    {
        gameManager = GameManager.Instance;
        countdownTimer = countdownTime;
        currentPlayerTurn = 0;
        gameStarted = false;
        gameEnded = false;
        playerOneTime = 0f;
        playerTwoTime = 0f;
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

        if (currentPlayerTurn == 1)
            playerOneTime += Time.deltaTime;
        else
            playerTwoTime += Time.deltaTime;

        if (playerOneTime + playerTwoTime >= totalTimeLimit)
        {
            EndGame();
            return;
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        if (playerOneTimeText != null)
            playerOneTimeText.text = FormatTime(playerOneTime);
        if (playerTwoTimeText != null)
            playerTwoTimeText.text = FormatTime(playerTwoTime);
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
        currentPlayerTurn = 1;

        playerOne.SetActive(true);
        playerTwo.SetActive(false);
    }

    public void OnPlayerClick(DevilPlayer player)
    {
        if (!gameStarted || gameEnded) return;

        int playerNumber = (player == playerOne) ? 1 : 2;

        if (playerNumber == currentPlayerTurn)
        {
            ApplyPenalty(player);
            return;
        }

        SwitchTurn();
    }

    void SwitchTurn()
    {

        if (currentPlayerTurn == 1)
        {
            currentPlayerTurn = 2;
            playerOne.SetActive(false);
            playerTwo.SetActive(true);
        }
        else
        {
            currentPlayerTurn = 1;
            playerOne.SetActive(true);
            playerTwo.SetActive(false);
        }
    }

    void ApplyPenalty(DevilPlayer player)
    {
        if (player == playerOne)
        {
            playerOneTime = Mathf.Max(0f, playerOneTime - outOfOrderPenalty);
            playerOne.OnPenalty();
        }
        else
        {
            playerTwoTime = Mathf.Max(0f, playerTwoTime - outOfOrderPenalty);
            playerTwo.OnPenalty();
        }

    }

    void EndGame()
    {
        gameEnded = true;

        playerOne.SetActive(false);
        playerTwo.SetActive(false);

        if (playerOneTimeText != null)
        {
            playerOneTimeText.text = FormatTime(playerOneTime);
        }
        if (playerTwoTimeText != null)
        {
            playerTwoTimeText.text = FormatTime(playerTwoTime);
        }

        if (playerOneTime > playerTwoTime)
        {
            playerOne.SetResult(1); // Success
            playerTwo.SetResult(3); // Failure
        }
        else if (playerTwoTime > playerOneTime)
        {
            playerOne.SetResult(3); // Failure
            playerTwo.SetResult(1); // Success
        }
        //pretty sure this else is basically impossible but if we want to switch to a diff scoring than just raw comparing the floats this might be useful
        else
        {
            playerOne.SetResult(2); // Neutral
            playerTwo.SetResult(2); // Neutral
        }

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
