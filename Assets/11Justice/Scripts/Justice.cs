using UnityEngine;

public class Justice : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Rigidbody2D rigidBody;
    private float yBound;
    public PlayerData player1Data;
    public PlayerData player2Data;

    [SerializeField]GameObject player1;
    [SerializeField]GameObject player2;
    
    GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
        player1Data = GameObject.FindGameObjectWithTag("Player1Data").GetComponent<PlayerData>();
        player2Data = GameObject.FindGameObjectWithTag("Player2Data").GetComponent<PlayerData>();
        
        //loss = False;
    }

    // Update is called once per frame
    /*
    void Update()
    {
        if (true){
            rigidBody.linearVelocity = Vector2.zero;
            rigidBody.AddForce(Vector2.up);
        }
        if (true) { //change this to actual collision with pipes
            player1Data.SetBufferState(3); //change to specific player etc etc
            player2Data.SetBufferState(3); //3 = failed game, 1 = won game (how do u win flappy bird though)
            gameManager.FinishMinigame(); //going to have to update everything to its own function
        }
    }
    */

    public void GameOver()
    {
        Time.timeScale = 0f;
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

Look Into:
- Game objects
    - set active function
- box collider 2D (probably going to be in tutorial)
- overivew of unity tutorial 
- Format of game
    - each player plays flappy bird on their own 
    - split screen or no?
    - would programming only need to account for one player and then you duplicate it?
    - winning:
        - option 1: have set amount of pipes to go through and person who gets through most wins(takes away from flappy bird vibe, extra programming)
        - option 2: everyone plays - ppl lose as game goes on, last person to keep going wins (might take a while and go past 60 seconds but unlikely to do so)
    
- 
*/