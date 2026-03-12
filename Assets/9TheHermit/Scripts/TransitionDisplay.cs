using UnityEngine;

public class TransitionDisplay : MonoBehaviour
{
    
    PlayerData playerData;
    [SerializeField] string playerName = "";

    Animator anim;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerData = GameObject.FindGameObjectWithTag(playerName).GetComponent<PlayerData>();
        anim = GetComponent<Animator>();

        if (playerData.GetBufferState() == 1)
        {
            anim.SetTrigger("Great");
        }
        else if (playerData.GetBufferState() == 2)
        {
            anim.SetTrigger("Overcooked");
        }
        else if (playerData.GetBufferState() == 3)
        {
            anim.SetTrigger("Raw");
        }
    }
}
