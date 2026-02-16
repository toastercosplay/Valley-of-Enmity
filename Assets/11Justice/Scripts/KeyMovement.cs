using UnityEngine;

public class KeyMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float velocity = 2;
    private Rigidbody2D key;

    void Start()
    {
        key = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            key.linearVelocity = new Vector2(0, velocity);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //add end game stuff
    }
}
