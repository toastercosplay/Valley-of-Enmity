using UnityEngine;

public class Death : MonoBehaviour
{
    
    [SerializeField] DeathPlayer player1;
    [SerializeField] DeathPlayer player2;
    //[SerializeField] DeathPlayer player3;
    //[SerializeField] DeathPlayer player4;

    Transform targetPlayer;

    public int delay = 300;
    int seed;

    Quaternion rawRotation;

    Animator anim;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (delay > 0)
        {
            delay--;
        }
        else
        {
            locateAPlayer();
            delay = Random.Range(100, 400);
        }
    }

    void locateAPlayer()
    {
        seed = Random.Range(0, 2);

        if (seed == 0)
        {
            targetPlayer = player1.transform;
        }
        else if (seed == 1)
        {
            targetPlayer = player2.transform;
        }

        this.transform.position = targetPlayer.position;

        rawRotation = Random.rotation;
        rawRotation.x = 0;
        rawRotation.y = 0;

        this.transform.rotation = Random.rotation;
        anim.SetTrigger("Attack");
    }


}
