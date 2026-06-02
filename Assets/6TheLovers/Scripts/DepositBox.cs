using UnityEngine;

public class DepositBox : MonoBehaviour
{
    public int boxIndex;
    public LoverItem heldItem = null;
    [SerializeField] LoverPlayer loverPlayer;

    // A quick check to see if we have room
    public bool IsEmpty => heldItem == null;

    public void PlaceItem(LoverItem item)
    {
        heldItem = item;
        item.AssignToParent(this.transform);
        
        if (loverPlayer != null)
        {
            loverPlayer.inLetter[boxIndex] = item;
        }
    }

    public LoverItem TakeItem()
    {
        if (heldItem == null) return null;

        LoverItem itemToTake = heldItem;
        heldItem = null;

        if (loverPlayer != null)
        {
            loverPlayer.inLetter[boxIndex] = null;
        }

        return itemToTake;
    }
}
