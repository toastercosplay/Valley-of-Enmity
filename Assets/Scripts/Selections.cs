using UnityEngine;
using UnityEngine.InputSystem;

public class Selections : MonoBehaviour
{
    int selectionState = 0; //0 = nothing, 1 = player count, 2 = player characters, 3 = game count
    int tempSelection = 2;
    private bool canMove = true;
    bool hasDoneIt = false;

    Animator anim;

    GameManager gameManager;

    PlayerInput playerInput;

    [SerializeField] GameObject hand1;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        gameManager = GameManager.Instance;

        playerInput = GetComponent<PlayerInput>();

        //Debug.Log("Game Manager instance: " + gameManager);
    }

    void Update()
    {
        if (selectionState == 3)
        {
            if (tempSelection != 3 && tempSelection != 5)
            {
                tempSelection = 3;
            }
        }
        
        anim.SetInteger("SelectionState", selectionState);
        anim.SetInteger("TempSelection", tempSelection);

        //Debug.Log(selectionState);

        if (selectionState == 2 && !hasDoneIt)
        {
            selectionState = 3;
            hasDoneIt = true;
            //Debug.Log("Set temp selection to 3");
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        float move = context.ReadValue<Vector2>().x;

        // Deadzone threshold for releasing movement
        const float threshold = 0.3f;

        if (selectionState == 1)
        {
            // Allow input only when stick is newly tilted past threshold
            if (canMove)
            {
                if (move > threshold && tempSelection < 4)
                {
                    tempSelection++;
                    canMove = false;
                }
                else if (move < -threshold && tempSelection > 2)
                {
                    tempSelection--;
                    canMove = false;
                }
            }

            // Reset movement lock when stick returns to neutral
            if (Mathf.Abs(move) < threshold)
            {
                canMove = true;
            }
        }
        else if (selectionState == 3)
        {
            if (canMove)
            {
                if (move > threshold && tempSelection == 3)
                {
                    tempSelection = 5;
                    canMove = false;
                }
                else if (move < -threshold && tempSelection == 5)
                {
                    tempSelection = 3;
                    canMove = false;
                }
            }

            if (Mathf.Abs(move) < threshold)
            {
                canMove = true;
            }
        }
    }

    public void OnA(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (selectionState == 0)
            {
                selectionState = 1;
            }
            else if (selectionState == 1)
            {
                if (tempSelection >= 2 && tempSelection <= 3)
                {
                    gameManager.SetNumberOfPlayers(tempSelection);
                    selectionState = 2;
                }

                playerInput.user.UnpairDevices();
                hand1.SetActive(true);
                this.gameObject.SetActive(false);
            }
            else if (selectionState == 2)
            {
                Debug.Log("something is wrong here");
            }
            else if (selectionState == 3)
            {
                if (tempSelection == 3 || tempSelection == 5)
                {
                    gameManager.SetNumberOfGames(tempSelection);
                    gameManager.StartGame();
                }
            }
            //Debug.Log("A button pressed");
        }
    }

    public void OnB(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (selectionState == 3)
            {
                selectionState = 2;
                //tempSelection = 2;
            }
            if (selectionState == 2)
            {
                selectionState = 1;
                tempSelection = 2;
            }
            else if (selectionState == 1)
            {
                selectionState = 0;
                tempSelection = 2;
            }
        }
    }
}
