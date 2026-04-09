using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;

public class Selections : MonoBehaviour
{
    Animator anim;
    GameManager gameManager;
    [SerializeField] GameObject CountSelectionObject;
    [SerializeField] PlayerInput playerInput;

    void Start()
    {
        anim = GetComponent<Animator>();
        gameManager = GameManager.Instance;

        //Debug.Log("Game Manager instance: " + gameManager);
    }

    public void OnA(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            
            CountSelectionObject.SetActive(true);
            this.gameObject.SetActive(false);

            playerInput.SwitchCurrentActionMap("CountSelection");
        }
    }
}
