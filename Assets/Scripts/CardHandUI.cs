using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CardHandUI : MonoBehaviour
{
    //cards from left to right
    [SerializeField] private RectTransform[] cards;

    [SerializeField] private float fanSpacing = 140f;
    [SerializeField] private float raisedHeight = 40f;

    [SerializeField] private float moveThreshold = 0.3f;

    private int selectedIndex = 0;
    private float moveCooldown = 0.2f;
    private float moveTimer;

    [SerializeField] string playerName = "";
    PlayerData playerData;

    [SerializeField] GameObject objectToOn;

    PlayerInput playerInput;

    void Start()
    {
        //set playerdata to first object with the tag playerName
        playerData = GameObject.FindGameObjectWithTag(playerName).GetComponent<PlayerData>();

        playerInput = GetComponent<PlayerInput>();

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
        if (selectedIndex == 0)
        {
            playerData.SetCharacter(0);
        }
        else if (selectedIndex == 1)
        {
            playerData.SetCharacter(0);
        }
        else if (selectedIndex == 2)
        {
            playerData.SetCharacter(0);
        }
        else if (selectedIndex == 3)
        {
            playerData.SetCharacter(0);
        }

        playerInput.user.UnpairDevices();
        objectToOn.SetActive(true);
        this.gameObject.SetActive(false);

    }

    void ChangeSelection(int direction)
    {
        selectedIndex = Mathf.Clamp(selectedIndex + direction, 0, cards.Length - 1);
        moveTimer = moveCooldown;
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
            cards[i].SetAsLastSibling(); // ensures selected renders on top
        }
    }
}
