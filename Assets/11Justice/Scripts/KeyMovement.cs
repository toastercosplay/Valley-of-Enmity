using UnityEngine;

public class KeyMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float velocity = 6.5f;
    private Rigidbody2D key;
    public int playerIndex = 0;
    private bool isAlive = true; 
    [SerializeField] Justice justice;


    void Start()
    {
        key = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
           //Fly(); 
        //}
        //make into Fly function and then use using button script and attach player/fly function together
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //add end game stuff
        isAlive = false;
        GetComponent<Rigidbody2D>().simulated = false;
        GetComponent<Collider2D>().enabled = false;
        justice.PlayerDied(playerIndex);
    }

    public void Fly(){
        key.linearVelocity = new Vector2(0, velocity);
    }
}
