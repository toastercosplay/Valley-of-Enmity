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
            currentItem.BePickedUp(transform.position, this.gameObject);
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

            currentItem.BePickedUp(transform.position, this.gameObject);
            isHoldingItem = true;
            hoveredItem = null;
            return;
        }
        else if (isHoldingItem && currentItem != null)
        {
            //drop item
            DepositBox box = GetOverlappingBox();

            if (box != null && box.holding == null)
            {
                box.PlaceItem(currentItem);
            }
            else
            {
                Vector2 dropOffset = new Vector2(0f, -1f);
                Vector2 newItemPosition = (Vector2)transform.position + dropOffset;
                currentItem.BePickedUp(newItemPosition, this.gameObject);
            }

            isHoldingItem = false;
            currentItem = null;

        }        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item") && !isHoldingItem)
        {
            Collectible item = other.gameObject.GetComponent<Collectible>();
            
            // Only allow hovering if the item is currently unowned
            if (item != null && item.currentOwner == null)
            {
                hoveredItem = item;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
            hoveredItem = null;
            return;
    }

    private DepositBox GetOverlappingBox()
    {
        // You can use a small OverlapSphere or check a cached reference
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.5f);
        foreach (var hit in hitColliders)
        {
            if (hit.TryGetComponent<DepositBox>(out DepositBox box))
            {
                return box;
            }
        }
        return null;
    }
}
