using UnityEngine;
using System.Collections;

public class EndGameAtTime : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField] float timeToEndGame = 30f;
    [SerializeField] float transitionTime = 5f;

    [SerializeField] GameObject[] transitionObjects;
    
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    IEnumerator EndGameAfterTime()
    {
        yield return new WaitForSeconds(timeToEndGame + transitionTime);
        gameManager.FinishMinigame();
    }

    IEnumerator TransitionAfterTime()
    {
        yield return new WaitForSeconds(timeToEndGame);
        foreach (var obj in transitionObjects)
        {
            obj.SetActive(true);
        }
    }

}
