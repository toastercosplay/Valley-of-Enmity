using UnityEngine;

public class LoverGame : MonoBehaviour
{
    bool needsRose = false;
    bool needsClover = false;
    bool needsSunflower = false;
    
    bool needsUnicorn = false;
    bool needsDragon = false;
    bool needsCat = false;

    bool needsHat = false;
    bool needsBoots = false;
    bool needsMustache = false;
    
    bool needsGun = false;
    bool needsWhip = false;
    bool needsWand = false;

    int amountCorrect = 0;

    Animator anim;

    [SerializeField]GameObject roseObject;
    [SerializeField]GameObject cloverObject;
    [SerializeField]GameObject sunflowerObject;
    [SerializeField]GameObject unicornObject;
    [SerializeField]GameObject dragonObject;
    [SerializeField]GameObject catObject;
    [SerializeField]GameObject hatObject;
    [SerializeField]GameObject bootsObject;
    [SerializeField]GameObject mustacheObject;
    [SerializeField]GameObject gunObject;
    [SerializeField]GameObject whipObject;
    [SerializeField]GameObject wandObject;

    [SerializeField]GameObject player1;
    [SerializeField]GameObject player2;

    [SerializeField] GameObject instructions;

    GameManager gameManager;
    PlayerData player1Data;
    PlayerData player2Data;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.Instance;
        player1Data = GameObject.FindGameObjectWithTag("Player1Data").GetComponent<PlayerData>();
        player2Data = GameObject.FindGameObjectWithTag("Player2Data").GetComponent<PlayerData>();

        int seed = Random.Range(0, 81);

        //maybe some sort of trinary tree ???

    }

    void UpdateVisuals()
    {
        if (needsRose)
        {
            roseObject.SetActive(true);
        }
        else if (needsClover)
        {
            cloverObject.SetActive(true);
        }
        else if (needsSunflower)
        {
            sunflowerObject.SetActive(true);
        }

        if (needsUnicorn)
        {
            unicornObject.SetActive(true);
        }
        else if (needsDragon)
        {
            dragonObject.SetActive(true);
        }
        else if (needsCat)
        {
            catObject.SetActive(true);
        }

        if (needsHat)
        {
            hatObject.SetActive(true);
        }
        else if (needsBoots)
        {
            bootsObject.SetActive(true);
        }
        else if (needsMustache)
        {
            mustacheObject.SetActive(true);
        }

        if (needsGun)
        {
            gunObject.SetActive(true);
        }
        else if (needsWhip)
        {
            whipObject.SetActive(true);
        }
        else if (needsWand)
        {
            wandObject.SetActive(true);
        }

    }

}
