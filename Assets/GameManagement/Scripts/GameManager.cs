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
    public GameObject playerConfig;

    [SerializeField] public PlayerData player1;
    [SerializeField] public PlayerData player2;
    [SerializeField] public PlayerData player3;
    [SerializeField] public PlayerData player4;

    
    [Header("Deck things")]
    public List<GameObject> cardList;
     Transform deckTransform;
    public float drawSpeed = 0.4f;
    public float flipSpeed = 0.3f;
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
        if (deckTransform == null)
        {
            deckTransform = GameObject.FindGameObjectWithTag("DECK").GetComponent<Transform>();
        }
        
        
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

        if (deckTransform != null)
        {
            Vector3 originalDeckPos = deckTransform.position;
            for (int i = 0; i < 5; i++)
            {
                deckTransform.position = originalDeckPos + (Vector3)UnityEngine.Random.insideUnitCircle * 0.1f;
                yield return new WaitForSeconds(0.04f);
            }
            deckTransform.position = originalDeckPos;
        }

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

        Vector3 startPos = deckTransform != null ? deckTransform.position : Vector3.zero;
        GameObject spawnedCardObj = Instantiate(selectedCardObject, startPos, Quaternion.identity);
        Card selectedCard = spawnedCardObj.GetComponent<Card>();

        selectedCard.SetFaceDown();
        cardList.RemoveAt(index);
        //PlaySound();

        float elapsed = 0f;
        while (elapsed < drawSpeed)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / drawSpeed;
            float smoothStep = t * t * (3f - 2f * t); 
            
            spawnedCardObj.transform.position = Vector3.Lerp(startPos, spawnPosition, smoothStep);
            yield return null;
        }
        spawnedCardObj.transform.position = spawnPosition;

        elapsed = 0f;
        float halfFlipSpeed = flipSpeed / 2f;

        while (elapsed < halfFlipSpeed)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfFlipSpeed;
            spawnedCardObj.transform.rotation = Quaternion.Euler(0f, Mathf.Lerp(0f, 90f, t), 0f);
            yield return null;
        }

        selectedCard.SetFaceUp();

        elapsed = 0f;
        while (elapsed < halfFlipSpeed)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfFlipSpeed;
            spawnedCardObj.transform.rotation = Quaternion.Euler(0f, Mathf.Lerp(90f, 0f, t), 0f);
            yield return null;
        }
        spawnedCardObj.transform.rotation = Quaternion.identity;

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

    public Selections selectionsMenu; // Drag your Selections object here in the inspector
    public GameObject[] characterSelectScreens;
    public void StartCharacterSelection()
    {
        // Turn on the correct number of character select screens based on player count
        for (int i = 0; i < GetNumberOfPlayers(); i++)
        {
            characterSelectScreens[i].SetActive(true);
        }
    }

    public void CompleteCharacterSelection()
    {
        Debug.Log("All players selected characters. Moving to Game Mode Select.");
        // Call the method we just added to Selections.cs!
        selectionsMenu.ShowGameModeSelect(); 
    }
}
