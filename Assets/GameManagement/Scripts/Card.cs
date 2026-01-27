using UnityEngine;
using UnityEngine.SceneManagement;

public class Card : MonoBehaviour
{
    [SerializeField] string cardName;

    Transform cardTransform;
    SpriteRenderer cardSpriteRenderer;

    void Start()
    {
        cardTransform = GetComponent<Transform>();
        cardSpriteRenderer = GetComponent<SpriteRenderer>();
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        Scene scene = SceneManager.GetActiveScene();
        
        if (scene.name == "Table")
        {
            cardSpriteRenderer.enabled = true;
        }
        else
        {
            cardSpriteRenderer.enabled = false;
        }
    }

    public void setSlot(int slotNum)
    {
        //setting the slot number for the card
        //tens digit is total number of cards
        //ones digit is the slot number

        //3-card table positions
        if (slotNum == 31)
        {
            cardTransform.localPosition = new Vector3(-1, -.6f, 0);
        }
        else if (slotNum == 32)
        {
            cardTransform.localPosition = new Vector3(2.5f, .6f, 0);
        }
        else if (slotNum == 33)
        {
            cardTransform.localPosition = new Vector3(6, -.6f, 0);
        }

    }

    public string getCardName()
    {
        return cardName;
    }
}
