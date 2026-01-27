using UnityEngine;

public class TheFool : MonoBehaviour
{
    [SerializeField] int timeLeft = 600;
    bool readyToEnd = false;
    
    [SerializeField] Scarecrow scarecrow;
    Vector2 targetPosition;

    [SerializeField] FoolPlayer playerOne;
    float distanceOne;
    [SerializeField] FoolPlayer playerTwo;
    float distanceTwo;

    GameManager gameManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       targetPosition = scarecrow.transform.position; 
       gameManager = GameManager.Instance;
    }

    void FixedUpdate()
    {
        if (playerOne.GetHasArrow() == false && playerTwo.GetHasArrow() == false)
        {
            finalJudgement();
        }

        if (timeLeft > 0)
        {
            timeLeft--;
        }
        else if (readyToEnd == false)
        {
            if (playerOne.GetHasArrow() == true)
            {
                playerOne.ShootArrow();
            }
            if (playerTwo.GetHasArrow() == true)
            {
                playerTwo.ShootArrow();
            }
            
            readyToEnd = true;
            timeLeft = 120;
            finalJudgement();
        }
        else
        {
            gameManager.FinishMinigame();
        }
    }

    public float submitPosition(Vector2 position)
    {
        return Vector2.Distance(position, targetPosition); 
    }

    public void submitDistance(float distance, string name)
    {
        if (name == "Player1Data")
        {
            distanceOne = distance;
        }
        else if (name == "Player2Data")
        {
            distanceTwo = distance;
        }
    } 

    void finalJudgement()
    {
        if (distanceOne < distanceTwo)
        {
            playerOne.playerData.SetBufferState(1);
            playerTwo.playerData.SetBufferState(3);
        }
        else if (distanceTwo < distanceOne)
        {
            playerTwo.playerData.SetBufferState(1);
            playerOne.playerData.SetBufferState(3);
        }
        else
        {
            playerOne.playerData.SetBufferState(2);
            playerTwo.playerData.SetBufferState(2);
        }
    }
}
