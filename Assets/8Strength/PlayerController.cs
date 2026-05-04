using System.Security;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float goalSatisfaction = 100;
    private float currentSatisfaction = 0;
    public float startingTimeLimit = 3f;
    public float minimumTimeLimit = 0.75f;
    public float speedIncrease = 0.15f;
    public float timeLimit;
    private float timer;
    public int wrongPenalty = 5;
    public int correctPoints = 3;

    void Start()
    {
        timeLimit = startingTimeLimit;
        
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            //WrongAction();
            return;
        }

        if (currentSatisfaction == goalSatisfaction)
        {
            // end game for everyone - player wins
        }
    }

    void WrongAction()
    {
        if (currentSatisfaction > 5)
        {
            currentSatisfaction = currentSatisfaction - wrongPenalty;
        }
        else
        {
            currentSatisfaction = 0;
        }

        //trustText.text = "Trust: " + trust;
    }

    void CorrectAction()
    {
        currentSatisfaction = currentSatisfaction + correctPoints;
        if (currentSatisfaction > goalSatisfaction)
        {
            //winning sequence
        }
    }

    void Action()
    {
        timer = timeLimit;

        int randomAction = Random.Range(0, 4);

        if (randomAction == 0)
        {
            //"Feed the horse an apple!";
            //currentCorrectButton = "A";
        }
        else if (randomAction == 1)
        {
            //promptText.text = "Brush the horse!";
            //currentCorrectButton = "B";
        }
        else if (randomAction == 2)
        {
            //promptText.text = "Give the horse water!";
            //currentCorrectButton = "X";
        }
        else if (randomAction == 3)
        {
            //promptText.text = "Pet the horse!";
            //currentCorrectButton = "Y";
        }
    }

    void feedApple()
    {
        
    }
}
