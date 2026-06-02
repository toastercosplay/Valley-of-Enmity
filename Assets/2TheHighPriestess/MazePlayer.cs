using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class MazePlayer : MonoBehaviour
{
    [SerializeField] MazeGenerator myMaze;
    [SerializeField] float moveDelay = 0.2f;

    private int currentX;
    private int currentY;
    private float nextMoveTime;
    private Vector2 inputDirection;

    [SerializeField] int playerNumber;

    public int berriesCollected = 0;
    private bool hasBerry = false;
    [SerializeField] GameObject berryIndicator;

    [SerializeField] string playerName = "";
    public PlayerData playerData;

    [SerializeField] TextMeshProUGUI scoreText;

    void Start()
    {
        playerData = GameObject.FindGameObjectWithTag(playerName).GetComponent<PlayerData>();
        
        if (myMaze == null)
        {
            return;
        }

        myMaze.playerList.Add(this);

        if (playerNumber == 1)
        {
            currentX = 9;
            currentY = 9; 
        }
        if (playerNumber == 2)
        {
            currentX = 11;
            currentY = 11; 
        }
        if (playerNumber == 3)
        {
            currentX = 11;
            currentY = 9; 
        }
        if (playerNumber == 4)
        {
            currentX = 9;
            currentY = 11; 
        }
        

        UpdateVisualPosition();
    }

    void Update()
    {
        if (Time.time >= nextMoveTime && inputDirection != Vector2.zero)
        {
            TryMove();
        }

        if (hasBerry)
        {
            berryIndicator.SetActive(true);
        }
        else
        {
            berryIndicator.SetActive(false);
        }

        scoreText.text = berriesCollected.ToString();
      
    }

    // Called by the Player Input component
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 rawInput = context.ReadValue<Vector2>();
    
        if (rawInput.magnitude <= .75f)
        {
            inputDirection = Vector2.zero; // Reset so they stop moving
            return;
        }

        inputDirection = rawInput;
    }

    void TryMove()
    {
        int moveX = 0;
        int moveY = 0;

        if (Mathf.Abs(inputDirection.x) > Mathf.Abs(inputDirection.y))
            moveX = inputDirection.x > 0 ? 1 : -1;
        else
            moveY = inputDirection.y > 0 ? 1 : -1;

        int targetX = currentX + moveX;
        int targetY = currentY + moveY;

        //Debug.Log(myMaze.GetValue(targetX, targetY));

        if (myMaze.GetValue(targetX, targetY) != 0)
        {
            currentX = targetX;
            currentY = targetY;

            // CHECK FOR BERRY
            if (myMaze.GetValue(targetX, targetY) == 2)
            {
                if (!hasBerry)
                {
                    myMaze.CollectBerry(currentX, currentY);
                    hasBerry = true;
                }
            }

            if (myMaze.GetValue(targetX, targetY) == 3)
            {
                if (hasBerry)
                {
                    myMaze.UpdateCollection();
                    berriesCollected += 1;
                    hasBerry = false;
                }
            }

            UpdateVisualPosition();
        }

        nextMoveTime = Time.time + moveDelay;
    }

    void UpdateVisualPosition()
    {
        transform.position = new Vector3(currentX, currentY, 0);
    }
}