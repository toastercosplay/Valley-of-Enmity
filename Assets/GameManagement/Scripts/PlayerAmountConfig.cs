using UnityEngine;

public class PlayerAmountConfig : MonoBehaviour
{
    int amount = 0;
    GameManager gameManager;

    //only one stage
    [SerializeField] GameObject TwoPStage;
    [SerializeField] GameObject ThreePStage;
    [SerializeField] GameObject FourPStage;

    //players just get enabled additionally
    [SerializeField] GameObject Player1;
    [SerializeField] GameObject Player2;
    [SerializeField] GameObject Player3;
    [SerializeField] GameObject Player4;

    public bool testing = false;
    public int testingAmount = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.Instance;
        amount = gameManager.GetNumberOfPlayers();

        if (testing)
        {
            amount = testingAmount;
        }

        if (TwoPStage != null && ThreePStage != null && FourPStage != null)
        {
            if (amount == 2)
            {
                TwoPStage.SetActive(true);
                ThreePStage.SetActive(false);
                FourPStage.SetActive(false);
            }
            else if (amount == 3)
            {
                TwoPStage.SetActive(false);
                ThreePStage.SetActive(true);
                FourPStage.SetActive(false);
            }
            else if (amount == 4)
            {
                TwoPStage.SetActive(false);
                ThreePStage.SetActive(false);
                FourPStage.SetActive(true);
            }
        }

        Player1.SetActive(true);
        if (amount >= 2)
        {
            Player2.SetActive(true);
        }
        if (amount >= 3)
        {
            Player3.SetActive(true);
        }
        if (amount >= 4)
        {
            Player4.SetActive(true);
        }

    }
}
