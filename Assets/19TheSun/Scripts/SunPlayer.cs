using UnityEngine;

public class SunPlayer : MonoBehaviour
{
    bool bullets = true;
    
    [SerializeField] TheSun theSun;

    bool victory = false;

    Animator anim;

    [SerializeField] string playerName = "";
    PlayerData playerData;
    
    void Start()
    {
        anim = GetComponent<Animator>();

        //set playerdata to first object with the tag playerName
        playerData = GameObject.FindGameObjectWithTag(playerName).GetComponent<PlayerData>();
    }

    public void Shoot()
    {
        //Debug.Log("You only got one bullet...");
        if (bullets)
        {
           //Debug.Log("You only got one bullet...");
           
            if (theSun.Evaluate())
            {
                victory = true;
                anim.SetTrigger("ShootGood");
            }
            else
            {
                anim.SetTrigger("ShootBad");
            }

            judgeVictory();
            bullets = false;
        }
    }

    public void judgeVictory()
    {
        if (victory)
        {
            playerData.SetBufferState(1);
        }
        else
        {
            playerData.SetBufferState(3);
        }
    }

    public bool HasBullets()
    {
        return bullets;
    }
}
