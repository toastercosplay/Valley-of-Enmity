using UnityEngine;

public class LoverPlayer : MonoBehaviour
{
    //this collectible class comes from the Temperance minigame, 
    //might as well reuse :)
    Collectible currentItem = null;
    Collectible hoveredItem = null;

    public bool isHoldingItem = false;
    [SerializeField] LoverGame loverGame = null;

    public Collectible[] inLetter = new Collectible[4];

    void Update()
    {
        if (isHoldingItem && currentItem != null)
        {
            currentItem.BePickedUp(transform.position);
        }
    }

    public void dropOrPickUp()
    {
        if (!isHoldingItem)
        {
            //try to pick up item
            if (hoveredItem == null)
            {
                return;
            }

            currentItem = hoveredItem.GetComponent<Collectible>();
            if (currentItem == null)
            {
                Debug.LogError("Hovered item has no Collectible component!");
                return;
            }

            currentItem.BePickedUp(transform.position);
            isHoldingItem = true;
            hoveredItem = null;
            return;
        }
        
        
        
        else if (isHoldingItem && currentItem != null)
        {
            //drop item
            Vector2 dropOffset = new Vector2(0f, -1f);
            Vector2 newItemPosition = (Vector2)transform.position + dropOffset;
            currentItem.BePickedUp(newItemPosition);
            
            isHoldingItem = false;
            currentItem = null;

        }        
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("LoverPlayer collided with: " + other.gameObject.name);

        if (other.gameObject.CompareTag("Item") && !isHoldingItem)
        {
            //isHoldingItem = true;
            hoveredItem = other.gameObject.GetComponent<Collectible>();
            return;
        }

        /*if (other.gameObject.CompareTag("DepositBox"))
        {

            if (!isHoldingItem)
                return;
            
            inLetter[other.gameObject.GetComponent<DepositBox>().boxIndex] = currentItem;
            other.gameObject.GetComponent<DepositBox>().holding = currentItem;
            
            isHoldingItem = false;
            currentItem = null;

            return;
        }*/
    }

    void OnTriggerExit(Collider other)
    {
            hoveredItem = null;
            return;
    }
}
