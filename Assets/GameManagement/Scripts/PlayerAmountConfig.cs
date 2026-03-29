using UnityEngine;

public class PlayerAmountConfig : MonoBehaviour
{
    int amount = 0;
    GameManager gameManager;

    [SerializeField] GameObject TwoPlayers;
    [SerializeField] GameObject ThreePlayers;
    [SerializeField] GameObject FourPlayers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.Instance;
        amount = gameManager.GetNumberOfPlayers();

        if (amount == 2)
        {
            TwoPlayers.SetActive(true);
            ThreePlayers.SetActive(false);
            FourPlayers.SetActive(false);
        }
        else if (amount == 3)
        {
            ThreePlayers.SetActive(true);
            TwoPlayers.SetActive(false);
            FourPlayers.SetActive(false);
        }
        else if (amount == 4)
        {
            FourPlayers.SetActive(true);
            TwoPlayers.SetActive(false);
            ThreePlayers.SetActive(false);
        }

    }
}
