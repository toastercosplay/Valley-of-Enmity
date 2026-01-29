using UnityEngine;

public class PotionPlayer : MonoBehaviour
{
    Collectible currentItem = null;
    bool isHoldingItem = false;
    [SerializeField] PotionBrewing potionBrewing = null;

    void Update()
    {
        if (isHoldingItem && currentItem != null)
        {
            currentItem.BePickedUp(transform.position);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("PotionPlayer collided with " + other.gameObject.name);
        
        if (other.gameObject.CompareTag("Item") && !isHoldingItem)
        {
            isHoldingItem = true;
            currentItem = other.gameObject.GetComponent<Collectible>();
            return;
        }

        if (other.gameObject.CompareTag("DepositBox"))
        {
            if (potionBrewing != null)
            {
                potionBrewing.addItem(currentItem);
                isHoldingItem = false;
                currentItem.Destroying();
                currentItem = null;
            }
            return;
        }
    }

    public void dropItem()
    {
        if (currentItem == null)
            return;

        //offset item slightly down
        Vector2 dropOffset = new Vector2(0f, -4f);
        Vector2 newItemPosition = (Vector2)transform.position + dropOffset;
        currentItem.BePickedUp(newItemPosition);
        
        isHoldingItem = false;
        currentItem = null;
    }
}
