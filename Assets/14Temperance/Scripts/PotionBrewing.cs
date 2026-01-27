using UnityEngine;

public class PotionBrewing : MonoBehaviour
{
    bool hasFlower = false;
    bool needsFlower1 = false;
    bool needsFlower2 = false;
    
    bool hasBirdPart = false;
    bool needsEgg = false;
    bool needsFeather = false;
    
    bool hasBottle = false;
    bool needsGreenBottle = false;
    bool needsRedBottle = false;
    bool needsBrownBottle = false;

    bool hasBone = false;
    bool needsSkull = false;

    int amountCorrect = 0;

    Animator anim;

    [SerializeField]GameObject flower1;
    [SerializeField]GameObject flower2;
    [SerializeField]GameObject egg;
    [SerializeField]GameObject feather;
    [SerializeField]GameObject greenBottle;
    [SerializeField]GameObject redBottle;
    [SerializeField]GameObject brownBottle;
    [SerializeField]GameObject skull;

    [SerializeField]GameObject player1;
    [SerializeField]GameObject player2;

    [SerializeField] GameObject instructions;

    GameManager gameManager;
    PlayerData player1Data;
    PlayerData player2Data;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        gameManager = GameManager.Instance;

        player1Data = GameObject.FindGameObjectWithTag("Player1Data").GetComponent<PlayerData>();
        player2Data = GameObject.FindGameObjectWithTag("Player2Data").GetComponent<PlayerData>();

        int seed = Random.Range(0, 11);

        Debug.Log("Potion seed: " + seed);

        //half flower 1 and half flower 2
        if (seed % 2 == 0)
        {
            needsFlower1 = true;
        }
        else
        {
            needsFlower2 = true;
        }

        //also half egg and half feather
        if (seed < 6)
        {
            needsEgg = true;
        }
        else
        {
            needsFeather = true;
        }

        //one third green bottle, one third red bottle, one third brown bottle
        if (seed%3 == 0)
        {
            needsGreenBottle = true;
        }
        else if (seed%3 == 1)
        {
            needsRedBottle = true;
        }
        else
        {
            needsBrownBottle = true;
        }

        //change if we add more bones
        needsSkull = true;

        UpdateVisuals();

    }

    void Update()
    {
        anim.SetInteger("AmountCorrect", amountCorrect);
    }

    void UpdateVisuals()
    {
        if (needsFlower1)
        {
            flower1.SetActive(true);
        }
        else if (needsFlower2)
        {
            flower2.SetActive(true);
        }

        if (needsEgg)
        {
            egg.SetActive(true);
        }
        else if (needsFeather)
        {
            feather.SetActive(true);
        }

        if (needsGreenBottle)
        {
            greenBottle.SetActive(true);
        }
        else if (needsRedBottle)
        {
            redBottle.SetActive(true);
        }
        else if (needsBrownBottle)
        {
            brownBottle.SetActive(true);
        }

        if (needsSkull)
        {
            skull.SetActive(true);
        }
    }

    void StartBrewing()
    {
        player1.SetActive(true);
        player2.SetActive(true);
        instructions.SetActive(false);
    }

    public void addItem(Collectible item)
    {
        if (!hasFlower)
        {
            if (item.itemName == "Flower1")
            {
                hasFlower = true;
                if (needsFlower1)
                {
                    amountCorrect++;
                }
                else
                {
                    amountCorrect--;
                }
            }
            if (item.itemName == "Flower2")
            {
                hasFlower = true;
                if (needsFlower2)
                {
                    amountCorrect++;
                }
                else
                {
                    amountCorrect--;
                }
            }
        }

        if (!hasBirdPart)
        {
            if (item.itemName == "Egg")
            {
                hasBirdPart = true;
                if (needsEgg)
                {
                    amountCorrect++;
                }
                else
                {
                    amountCorrect--;
                }
            }
            if (item.itemName == "Feather")
            {
                hasBirdPart = true;
                if (needsFeather)
                {
                    amountCorrect++;
                }
                else
                {
                    amountCorrect--;
                }
            }
        }

        if (!hasBottle)
        {
            if (item.itemName == "GreenBottle")
            {
                hasBottle = true;
                if (needsGreenBottle)
                {
                    amountCorrect++;
                }
                else
                {
                    amountCorrect--;
                }
            }
            if (item.itemName == "RedBottle")
            {
                hasBottle = true;
                if (needsRedBottle)
                {
                    amountCorrect++;
                }
                else
                {
                    amountCorrect--;
                }
            }
            if (item.itemName == "BrownBottle")
            {
                hasBottle = true;
                if (needsBrownBottle)
                {
                    amountCorrect++;
                }
                else
                {
                    amountCorrect--;
                }
            }
        }

        if (!hasBone)
        {
            if (item.itemName == "Skull")
            {
                hasBone = true;
                if (needsSkull)
                {
                    amountCorrect++;
                }
                else
                {
                    amountCorrect--;
                }
            }
        }
    }

    public void checkCompletion()
    {
        if (amountCorrect < 1)
        {
            //failure state
            player1Data.SetBufferState(3);
            player2Data.SetBufferState(3);
        }
        else if (amountCorrect == 4)
        {
            //success state
            player1Data.SetBufferState(1);
            player2Data.SetBufferState(1);
        }
        else
        {
            //incomplete state
            player1Data.SetBufferState(2);
            player2Data.SetBufferState(2);
        }

        gameManager.FinishMinigame();
    }
}
