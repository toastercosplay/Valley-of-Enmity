using UnityEngine;
using System.Collections;

public class EndGameAtTime : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField] float timeToEndGame = 30f;
    
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    IEnumerator EndGameAfterTime()
    {
        yield return new WaitForSeconds(timeToEndGame);
        gameManager.FinishMinigame();
    }

}
