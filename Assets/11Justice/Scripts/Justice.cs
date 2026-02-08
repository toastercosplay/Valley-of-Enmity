using UnityEngine;

public class Justice : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Rigidbody2D rigidBody;
    private float yBound;
    public PlayerData player1Data;
    public PlayerData player2Data;
    GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
        player1Data = GameObject.FindGameObjectWithTag("Player1Data").GetComponent<PlayerData>();
        player2Data = GameObject.FindGameObjectWithTag("Player2Data").GetComponent<PlayerData>();

        loss = False;
    }

    // Update is called once per frame
    void Update()
    {
        if (UsingButtons.onAPressed()){
            rigidBody.linearVelocity = Vector2.zero;
            rigidBody.AddForce(Vector2.up);
        }
        if (loss == True) { //change this to actual collision with pipes
            player1Data.SetBufferState(3); //change to specific player etc etc
            player2Data.SetBufferState(3); 
            gameManager.FinishMinigame(); //going to have to update everything to its own function
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        //should be for when bird hits pipes?
        
    }
}

/* 
TO DO:
[] unity flappy bird tutorial watch
[] understand current code
[] implement pipes 
    [] script
    [] unity
[] implement players
    [] script
    [] unity
[] look at animations
*/