using UnityEngine;

public class Emperor : MonoBehaviour
{
    [SerializeField] PlayerData player1;
    [SerializeField] PlayerData player2;
    [SerializeField] PlayerData player3;
    [SerializeField] PlayerData player4;
    
    [SerializeField] private int HealthPoints = 10;
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            HealthPoints--;
            //Debug.Log("Emperor hit! Remaining HP: " + HealthPoints);
            
            if (HealthPoints <= 0)
            {
                //Debug.Log("Emperor defeated!");
                // Add defeat logic here (e.g., end game, play animation, etc.)
            }
        }
    }
}
