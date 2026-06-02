using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;

public class Selections : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] GameObject TitleObject;
    [SerializeField] GameObject CountSelectionObject;
    [SerializeField] GameObject GameModeSelectionObject;
    
    [SerializeField] PlayerInput playerInput;

    int currentState = 0;

    void Start()
    {
        gameManager = GameManager.Instance;

        TitleObject.SetActive(true);
        CountSelectionObject.SetActive(false);
        GameModeSelectionObject.SetActive(false);
        playerInput.SwitchCurrentActionMap("Title");
    }

    public void OnA(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TitleObject.SetActive(false);
            CountSelectionObject.SetActive(true);
            playerInput.SwitchCurrentActionMap("CountSelection");
        }
    }

    public void ShowGameModeSelect()
    {
        this.gameObject.SetActive(true);
        CountSelectionObject.SetActive(false);
        GameModeSelectionObject.SetActive(true);
        playerInput.SwitchCurrentActionMap("GameSelection");
    }
}
