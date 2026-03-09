using UnityEngine;

public class DepositBox : MonoBehaviour
{
    [SerializeField] public int boxIndex;

    public Collectible holding = null;

    [SerializeField] LoverPlayer loverPlayer;

    public bool playerIsOverhead = false;

    // Update is called once per frame
    void Update()
    {
        if (holding != null)
        {
            holding.BePickedUp(transform.position);
        }
    }

    public void PlaceItem(Collectible item)
    {
        holding = item;
        loverPlayer.inLetter[boxIndex] = item;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) playerIsOverhead = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) playerIsOverhead = false;

        // If an item is pulled out of the box by the player
        if (other.gameObject.CompareTag("Item") && !loverPlayer.isHoldingItem)
        {
            holding = null;
            loverPlayer.inLetter[boxIndex] = null;
        }
    }


}
