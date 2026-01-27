using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class CursorMovement : MonoBehaviour
{
    float movementX, movementY;
    //[SerializeField] float speed = 10f;
    [SerializeField] float maxSpeed = 10f;

    private RectTransform imageRectTransform;

    public UnityEvent onAPressed;
    public UnityEvent onAReleased;
    public UnityEvent onRTPressed;
    public UnityEvent onRTReleased;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        imageRectTransform = GetComponent<RectTransform>();
    }

    void FixedUpdate()
    {
        imageRectTransform.anchoredPosition += new Vector2(movementX, movementY) * maxSpeed;
    }

    public void onMove(InputAction.CallbackContext context)
    {
        //on receiving messages from the input system
        Vector2 v = context.ReadValue<Vector2>();
        movementX = v.x / 10f;
        movementY = v.y / 10f;

        if (v.x < .2 && v.x > -.2)
        {
            movementX = 0;
        }
        if (v.y < .2 && v.y > -.2)
        {
            movementY = 0;
        }

        //Debug.Log(v.magnitude);
    }

    public void OnA(InputAction.CallbackContext context)
    {
        if (context.started)
            onAPressed.Invoke();

        if (context.canceled)
            onAReleased.Invoke();
    }

    public void OnRightTrigger(InputAction.CallbackContext context)
    {
        //Debug.Log("Right Trigger action detected.");
        
        if (context.started)
            onRTPressed.Invoke();

        if (context.canceled)
            onRTReleased.Invoke();
    }
}
