using UnityEngine;

public class KeyMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float velocity = 6.5f;
    private Rigidbody2D key;

    void Start()
    {
        key = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
           Fly(); 
        }
        //make into Fly function and then use using button script and attach player/fly function together
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //add end game stuff
        Time.timeScale = 0f;
    }

    public void Fly(){
        key.linearVelocity = new Vector2(0, velocity);
    }
}
