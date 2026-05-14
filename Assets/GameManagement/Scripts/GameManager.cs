using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    private int numberOfPlayers = 0;
    private int numberOfGames = 3;
    private int gamesPlayed = 0;
    //public GameObject playerConfig;

    [SerializeField] public PlayerData player1;
    [SerializeField] public PlayerData player2;
    [SerializeField] public PlayerData player3;
    [SerializeField] public PlayerData player4;

    public List<GameObject> cardList;
    public float spawnDelay = 5f;

    AudioSource audioSource;
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
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

        if (numberOfPlayers >= 3)
        {
            player3.gameObject.SetActive(true);
        }

        if (numberOfPlayers >= 4)
        {
            player4.gameObject.SetActive(true);
        }

        SceneManager.LoadScene("Table");
    }

    public void MakeSelection()
    {
        //Debug.Log("AH HELL NAW");
        
        if (gamesPlayed >= numberOfGames)
        {
            //Debug.Log("MAKINGSECSDLFJKASDF");
            return;
        }

        if (cardList == null || cardList.Count == 0)
        {
            //Debug.Log("AH HELL NAW");
            return;
        }

        //Debug.Log("all is well");
        
        StartCoroutine(SelectAndLoadRoutine());
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

    IEnumerator SelectAndLoadRoutine()
    {
        int index = Random.Range(0, cardList.Count);
        GameObject selectedCardObject = cardList[index];
        Card selectedCard = selectedCardObject.GetComponent<Card>();

        Vector3 spawnPosition = Vector3.zero;

        int ermmm = numberOfGames * 10 + (gamesPlayed + 1);
        //tens digit: total games
        //ones digit: current game 

        if (ermmm == 31)
            spawnPosition = new Vector3(-2f, -0.6f, 0);
        else if (ermmm == 32)
            spawnPosition = new Vector3(2.5f, 0.6f, 0);
        else if (ermmm == 33)
            spawnPosition = new Vector3(7f, -0.6f, 0);
        else if (ermmm == 51)
            spawnPosition = new Vector3(-2.5f, -1.5f, 0);
        else if (ermmm == 52)
            spawnPosition = new Vector3(0f, 0f, 0);
        else if (ermmm == 53)
            spawnPosition = new Vector3(2.5f, 1.5f, 0);
        else if (ermmm == 54)
            spawnPosition = new Vector3(5f, 0f, 0);
        else if (ermmm == 55)
            spawnPosition = new Vector3(7.5f, -1.5f, 0);


        Instantiate(selectedCardObject, spawnPosition, Quaternion.identity);
        PlaySound();

        cardList.RemoveAt(index);

        yield return new WaitForSeconds(spawnDelay);

        SceneManager.LoadScene(selectedCard.getCardName());
    }

    public void PlaySound()
    {
        audioSource.Play();
    }

    public int GetNumberOfPlayers()
    {
        return numberOfPlayers;
    }
}
