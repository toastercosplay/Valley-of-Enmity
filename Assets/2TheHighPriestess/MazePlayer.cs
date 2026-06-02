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

    //

    void Start()
    {
        if (myMaze == null)
        {
            return;
        }

        currentX = 1;// myMaze.startX;
        currentY = 1;//myMaze.startY;

        UpdateVisualPosition();
    }

    void Update()
    {
        if (Time.time >= nextMoveTime && inputDirection != Vector2.zero)
        {
            TryMove();
        }
    }

    // Called by the Player Input component
    void OnMove(InputValue value)
    {
        Debug.Log(value.Get<Vector2>().magnitude);

        if (value.Get<Vector2>().magnitude <= .75)
        {
            return;
        }
        
        inputDirection = value.Get<Vector2>();
        //Debug.Log("AHHHHH");
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
                myMaze.CollectBerry(currentX, currentY);
                //Debug.Log("Nom nom nom!");
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