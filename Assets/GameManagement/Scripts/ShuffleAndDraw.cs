using UnityEngine;
using System.Collections;

public class ShuffleAndDraw : MonoBehaviour
{
    GameManager gameManager;
    
    [SerializeField] float pauseBeforeDraw = 2.0f; 
    
    void Start()
    {
        gameManager = GameManager.Instance;
        
        StartCoroutine(AutoDrawRoutine());
    }

    IEnumerator AutoDrawRoutine()
    {
        yield return new WaitForSeconds(pauseBeforeDraw);
        
        Draw();
    }

    public void Draw()
    {
        if (gameManager != null)
        {
            gameManager.MakeSelection();
        }
        else
        {
            Debug.LogError("GameManager is missing!");
        }
    }
}