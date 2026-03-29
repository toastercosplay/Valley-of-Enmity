using UnityEngine;

public class EndOfTower : MonoBehaviour
{
    GameManager gameManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void endGame()
    {
        gameManager.FinishMinigame();
    }
}
