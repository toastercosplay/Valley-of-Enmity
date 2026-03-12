using UnityEngine;
using TMPro;

public class Scoring : MonoBehaviour
{
    
    PlayerData swordsPlayerData;
    [SerializeField] TMP_Text swordsTotalText;
    [SerializeField] TMP_Text swordsGainedText;
    int swordsState = 0;
    int swordsTotal = 0; //total score 
    int swordsScore = 0; //points just earned

    PlayerData wandsPlayerData;
    [SerializeField] TMP_Text wandsTotalText;
    [SerializeField] TMP_Text wandsGainedText;
    int wandsState = 0;
    int wandsTotal = 0;
    int wandsScore = 0;

    PlayerData cupsPlayerData;
    [SerializeField] TMP_Text cupsTotalText;
    [SerializeField] TMP_Text cupsGainedText;
    int cupsState = 0;
    int cupsTotal = 0;
    int cupsScore = 0;

    PlayerData pentaclesPlayerData;
    [SerializeField] TMP_Text pentaclesTotalText;
    [SerializeField] TMP_Text pentaclesGainedText;
    int pentaclesState = 0;
    int pentaclesTotal = 0;
    int pentaclesScore = 0;

    int updateTimer = 0;

    GameManager gameManager;
    AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        swordsPlayerData = GameObject.FindGameObjectWithTag("Player1Data").GetComponent<PlayerData>();
        wandsPlayerData = GameObject.FindGameObjectWithTag("Player2Data").GetComponent<PlayerData>();
        cupsPlayerData = GameObject.FindGameObjectWithTag("Player3Data").GetComponent<PlayerData>();
        pentaclesPlayerData = GameObject.FindGameObjectWithTag("Player4Data").GetComponent<PlayerData>();
        audioSource = GetComponent<AudioSource>();

        swordsTotal = swordsPlayerData.GetTotalScore();
        wandsTotal = wandsPlayerData.GetTotalScore(); //these will display initial scores
        cupsTotal = cupsPlayerData.GetTotalScore();
        pentaclesTotal = pentaclesPlayerData.GetTotalScore();

        gameManager = GameManager.Instance;

        retrieveFromBuffer();
        DrawScoreCards();
    }

    // Update is called once per frame
    void Update()
    {  
        //update scores once a second
        updateTimer++;
        if (updateTimer <= 30)
        {
            return;
        }

        swordsTotalText.text = "" + swordsTotal;
        swordsGainedText.text = "+" + swordsScore;

        wandsTotalText.text = "" + wandsTotal;
        wandsGainedText.text = "+" + wandsScore;  

        cupsTotalText.text = "" + cupsTotal;
        cupsGainedText.text = "+" + cupsScore;

        pentaclesTotalText.text = "" + pentaclesTotal;
        pentaclesGainedText.text = "+" + pentaclesScore;

        updateTimer = 0;

        //CHANGE LATER TO RANDOMIZE FROM THREE
        //PlaySound();
        
        //update totals one by one
        if (swordsTotal < swordsPlayerData.GetTotalScore())
        {
            swordsTotal++;
            //swordsScore--;
        }
        if (wandsTotal < wandsPlayerData.GetTotalScore())
        {
            wandsTotal++;
            //wandsScore--;
        }
        if (cupsTotal < cupsPlayerData.GetTotalScore())
        {
            cupsTotal++;
            //cupsScore--;
        }
        if (pentaclesTotal < pentaclesPlayerData.GetTotalScore())
        {
            pentaclesTotal++;
            //pentaclesScore--;
        }


        swordsScore--;
        wandsScore--;
        cupsScore--;
        pentaclesScore--;

        if (swordsScore < 1)
        {
            swordsGainedText.text = "";
        }
        if (wandsScore < 1)
        {
            wandsGainedText.text = "";
        }
        if (cupsScore < 1)
        {
            cupsGainedText.text = "";
        }
        if (pentaclesScore < 1)
        {
            pentaclesGainedText.text = "";
        }

        if (wandsScore < -2 && swordsScore < -2 && cupsScore < -2 && pentaclesScore < -2)
        {
            //both done updating
            gameManager.BackToTable();
        }

    }

    public void retrieveFromBuffer()
    {
        swordsState = swordsPlayerData.GetBufferState();
        wandsState = wandsPlayerData.GetBufferState();
        cupsState = cupsPlayerData.GetBufferState();
        pentaclesState = pentaclesPlayerData.GetBufferState();
    }

    public void DrawScoreCards()
    {
        if (swordsState == 1)
        {
            swordsScore = swordsPlayerData.Success();
        }
        else if (swordsState == 2)
        {
            swordsScore = swordsPlayerData.Neutral();
        }
        else if (swordsState == 3)
        {
            swordsScore = swordsPlayerData.Failure();
        }
        swordsPlayerData.SetBufferState(0);
        swordsPlayerData.SetTotalScore(swordsTotal + swordsScore);

        //wands player
        if (wandsState == 1)
        {
            wandsScore = wandsPlayerData.Success();
        }
        else if (wandsState == 2)
        {
            wandsScore = wandsPlayerData.Neutral();
        }
        else if (wandsState == 3)
        {
            wandsScore = wandsPlayerData.Failure();
        }
        wandsPlayerData.SetBufferState(0);
        wandsPlayerData.SetTotalScore(wandsTotal + wandsScore);

        //cups player
        if (cupsState == 1)
        {
            cupsScore = cupsPlayerData.Success();
        }
        else if (cupsState == 2)
        {
            cupsScore = cupsPlayerData.Neutral();
        }
        else if (cupsState == 3)
        {
            cupsScore = cupsPlayerData.Failure();
        }
        cupsPlayerData.SetBufferState(0);
        cupsPlayerData.SetTotalScore(cupsTotal + cupsScore);

        //pentacles player
        if (pentaclesState == 1)
        {
            pentaclesScore = pentaclesPlayerData.Success();
        }
        else if (pentaclesState == 2)
        {
            pentaclesScore = pentaclesPlayerData.Neutral();
        }
        else if (pentaclesState == 3)
        {
            pentaclesScore = pentaclesPlayerData.Failure();
        }
        pentaclesPlayerData.SetBufferState(0);
        pentaclesPlayerData.SetTotalScore(pentaclesTotal + pentaclesScore);
    }

    public void PlaySound()
    {
        audioSource.Play();
    }
}
