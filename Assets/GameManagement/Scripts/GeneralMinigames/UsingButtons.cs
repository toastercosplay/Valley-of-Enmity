using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class UsingButtons : MonoBehaviour
{
    public UnityEvent onAPressed;
    public UnityEvent onAReleased;

    public UnityEvent onBPressed;
    public UnityEvent onBReleased;

    public UnityEvent onXPressed;
    public UnityEvent onXReleased;

    public UnityEvent onYPressed;
    public UnityEvent onYReleased;

    public UnityEvent onRTPressed;
    public UnityEvent onRTReleased;
    
    
    public void OnA(InputAction.CallbackContext context)
    {
        if (context.started)
            onAPressed.Invoke();

        if (context.canceled)
            onAReleased.Invoke();
    }

    public void OnB(InputAction.CallbackContext context)
    {
        if (context.started)
            onBPressed.Invoke();

        if (context.canceled)
            onBReleased.Invoke();
    }

    public void OnX(InputAction.CallbackContext context)
    {
        if (context.started)
            onXPressed.Invoke();

        if (context.canceled)
            onXReleased.Invoke();
    }

    public void OnY(InputAction.CallbackContext context)
    {
        if (context.started)
            onYPressed.Invoke();

        if (context.canceled)
            onYReleased.Invoke();
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
