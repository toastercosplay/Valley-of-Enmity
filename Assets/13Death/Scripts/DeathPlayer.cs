using UnityEngine;

public class DeathPlayer : MonoBehaviour
{
    
    PlayerData playerdata;
    [SerializeField] string playerName = "";
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerdata = GameObject.FindGameObjectWithTag(playerName).GetComponent<PlayerData>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
