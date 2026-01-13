using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] string cardName;

    Transform cardTransform;

    void Start()
    {
        cardTransform = GetComponent<Transform>();
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
