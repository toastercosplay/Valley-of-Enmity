using UnityEngine;


//what a stupid script this is; i wish i didnt have to make this. 
public class ShuffleAndDraw : MonoBehaviour
{
    GameManager gameManager;
    
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void Draw()
    {
      gameManager.MakeSelection();
      //Debug.Log("Draw button pressed");
    }
}
