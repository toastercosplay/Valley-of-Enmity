using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class TopDownMovement : MonoBehaviour
{
    float movementX, movementY;
    //[SerializeField] float speed = 10f;
    [SerializeField] float maxSpeed = 10f;

    [SerializeField] UnityEvent onAButton;

    Rigidbody2D rb;
    Animator anim;

    float localXScale = 1;
    float localYScale = 1;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        localXScale = transform.localScale.x;
        localYScale = transform.localScale.y;
    }
    void FixedUpdate()
    {

        rb.linearVelocity = new Vector2(movementX, movementY) * maxSpeed;

        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }

        anim.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));
        if (rb.linearVelocity.x < 0)
        {
            transform.localScale = new Vector3(-localXScale, localYScale, 1);
        }
        else if (rb.linearVelocity.x > 0)
        {
            transform.localScale = new Vector3(localXScale, localYScale, 1);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
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

    public void OnA()
    {
        onAButton.Invoke();
    }
}
