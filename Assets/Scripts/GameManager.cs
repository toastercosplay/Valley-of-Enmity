using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private int numberOfPlayers = 0;
    private int numberOfGames = 0;
    private int gamesPlayed = 0;

    [SerializeField] public PlayerData player1;
    [SerializeField] public PlayerData player2;
    //[SerializeField] PlayerData player3;
    //[SerializeField] PlayerData player4;

    [SerializeField] List<Card> cardList;

    public static GameManager Instance;
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    public void SetNumberOfPlayers(int num)
    {
        numberOfPlayers = num;
    }

    public void SetNumberOfGames(int num)
    {
        numberOfGames = num;
    }

    public void StartGame()
    {
        player1.gameObject.SetActive(true);
        player2.gameObject.SetActive(true);
        SceneManager.LoadScene("14Temperance");
    }

    public void MakeSelection()
    {
        if (gamesPlayed >= numberOfGames)
        {
            // All games have been played
            return;
        }
        
        //selecting a minigame
        int index = Random.Range(0, cardList.Count);
        Card selected = cardList[index];

        int ermmm = numberOfGames * 10 + (gamesPlayed + 1);
        //selected.setSlot(ermmm);

        cardList.RemoveAt(index);

        SceneManager.LoadScene(selected.getCardName());
    }

    public void BackToTable()
    {
        gamesPlayed++;
        LoadScene("Table");
    }

    public void FinishMinigame()
    {
        LoadScene("Scoring");
    }
}
