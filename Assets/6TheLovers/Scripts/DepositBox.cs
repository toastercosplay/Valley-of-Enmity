using UnityEngine;

public class DepositBox : MonoBehaviour
{
    [SerializeField] public int boxIndex;

    public Collectible holding = null;

    [SerializeField] LoverPlayer loverPlayer;

    // Update is called once per frame
    void Update()
    {
        if (holding != null)
        {
            holding.BePickedUp(transform.position);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Item") && holding == null && loverPlayer.isHoldingItem == false)
        {
            Collectible item = other.gameObject.GetComponent<Collectible>();

            if (item == null)
            {
                Debug.LogError("Hovered item has no Collectible component!");
                return;
            }

            loverPlayer.inLetter[boxIndex] = item;

            holding = item;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            if (holding == null || loverPlayer.isHoldingItem)
                return;

            holding = null;
            loverPlayer.inLetter[boxIndex] = null;
        }
    }


}
