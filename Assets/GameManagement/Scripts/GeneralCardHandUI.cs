using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;

public class GeneralCardHandUI : MonoBehaviour
{
    //cards from left to right
    [SerializeField] private RectTransform[] cards;

    [SerializeField] private float fanSpacing = 140f;
    [SerializeField] private float raisedHeight = 40f;

    [SerializeField] private float moveThreshold = 0.3f;

    private int selectedIndex = 0;
    private float moveCooldown = 0.2f;
    private float moveTimer;

    GameManager gameManager;
    [SerializeField] string myType = "";

    [SerializeField] GameObject objectToOn;

    [SerializeField] PlayerInput playerInput;

    AudioSource audioSource;

    void Start()
    {
        //game manager
        gameManager = GameManager.Instance;

        //playerInput = GetComponent<PlayerInput>();
        Debug.Log("PlayerInput component: " + playerInput);

        UpdateLayout();
    }

    void Update()
    {
        moveTimer -= Time.unscaledDeltaTime;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!context.performed || moveTimer > 0f)
            return;

        float x = context.ReadValue<Vector2>().x;

        if (x > moveThreshold)
            ChangeSelection(1);
        else if (x < -moveThreshold)
            ChangeSelection(-1);
    }

    public void OnA(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        //card selection

        //FIX THIS LATERRRRRR !!!!!!!!!!!!!!!!!!
        if (myType == "Count")
        {
            if (selectedIndex == 0)
            {
                gameManager.SetNumberOfPlayers(2);
            }
            else if (selectedIndex == 1)
            {
                gameManager.SetNumberOfPlayers(3);
            }
            else if (selectedIndex == 2)
            {
                gameManager.SetNumberOfPlayers(4);
            }
        }
        if (myType == "Spread")
        {
            if (selectedIndex == 0)
            {
                gameManager.SetNumberOfGames(3);
            }
            else if (selectedIndex == 1)
            {
                gameManager.SetNumberOfGames(5);
            }
            else if (selectedIndex == 2)
            {
                gameManager.SetNumberOfGames(7); 
            }
            gameManager.StartGame();
        }

        //andrew spent hours here- fix this john later

        // foreach (var user in InputUser.all)
        // {
        //     user.UnpairDevices();
        // }
        playerInput.SwitchCurrentActionMap("Player1");

        objectToOn.SetActive(true);
        //stop rendering the children
        // for (int i = 0; i < cards.Length; i++)
        // {
        //     cards[i].GetComponent<Image>().enabled = false;
        // }
        //
        this.gameObject.SetActive(false);

    }

    void ChangeSelection(int direction)
    {
        selectedIndex = Mathf.Clamp(selectedIndex + direction, 0, cards.Length - 1);
        moveTimer = moveCooldown;
        PlaySound();
        UpdateLayout();
    }

    void UpdateLayout()
    {
        float startX = -(fanSpacing * (cards.Length - 1)) * 0.5f;

        for (int i = 0; i < cards.Length; i++)
        {
            float x = startX + i * fanSpacing;
            float y = (i == selectedIndex) ? raisedHeight : 0f;

            cards[i].anchoredPosition = new Vector2(x, y);
            cards[i].SetAsLastSibling(); //ensures selected renders on top
        }
    }

    public void PlaySound()
    {
        //audioSource.Play();
    }
}
