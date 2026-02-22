using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlatformerMovement : MonoBehaviour
{
    float movementX;
    //[SerializeField] float speed = 10f;
    [SerializeField] float maxSpeed = 10f;

    Rigidbody2D rb;
    Animator anim;

    float localXScale = 1;
    float localYScale = 1;

    [SerializeField] float jumpForce = 5f;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        localXScale = transform.localScale.x;
        localYScale = transform.localScale.y;
    }
    void FixedUpdate()
    {
        float moveVeloX = movementX * maxSpeed;

        rb.linearVelocity = new Vector2(moveVeloX, rb.linearVelocity.y);

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

        if (v.x < .2 && v.x > -.2)
        {
            movementX = 0;
        }

        //Debug.Log(v.magnitude);
        //Debug.Log($"Movement: {v}");
    }

    public void OnA()
    {
        //jump
        if (Mathf.Abs(rb.linearVelocity.y) < 0.1f) 
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
