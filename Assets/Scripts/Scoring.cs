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

    int updateTimer = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        swordsPlayerData = GameObject.FindGameObjectWithTag("Player2Data").GetComponent<PlayerData>();
        wandsPlayerData = GameObject.FindGameObjectWithTag("Player1Data").GetComponent<PlayerData>();

        swordsTotal = swordsPlayerData.GetTotalScore();
        wandsTotal = wandsPlayerData.GetTotalScore(); //these will display initial scores

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

        updateTimer = 0;
        
        //update totals one by one
        if (swordsTotal < swordsPlayerData.GetTotalScore())
        {
            swordsTotal++;
            swordsScore--;
        }
        if (wandsTotal < wandsPlayerData.GetTotalScore())
        {
            wandsTotal++;
            wandsScore--;
        }

        if (swordsScore < 1)
        {
            swordsGainedText.text = "";
        }
        if (wandsScore < 1)
        {
            wandsGainedText.text = "";
        }

    }

    public void retrieveFromBuffer()
    {
        swordsState = swordsPlayerData.GetBufferState();
        wandsState = wandsPlayerData.GetBufferState();
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
    }
}
