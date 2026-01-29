using UnityEngine;

public class LoverPlayer : MonoBehaviour
{
    bool hasFlower = false;
    bool hasAnimal = false;
    bool hasClothes = false;
    bool hasWeapon = false;
    
    //this collectible class comes from the Temperance minigame, 
    //might as well reuse :)
    Collectible currentItem = null;
    bool isHoldingItem = false;
    [SerializeField] LoverGame loverGame = null;

    void Update()
    {
        if (isHoldingItem && currentItem != null)
        {
            currentItem.BePickedUp(transform.position);
        }
    }

    public void dropItem()
    {
        if (currentItem == null)
            return;

        //offset item slightly down
        Vector2 dropOffset = new Vector2(0f, -1f);
        Vector2 newItemPosition = (Vector2)transform.position + dropOffset;
        currentItem.BePickedUp(newItemPosition);
        
        isHoldingItem = false;
        currentItem = null;
    }
}
