using UnityEngine;

public class Collectible : MonoBehaviour
{
    
    [SerializeField] public string itemName = "";
    public GameObject currentOwner = null;

    [SerializeField] float offsetX = 0f;
    [SerializeField] float offsetY = 0f;
    
    public void BePickedUp(Vector2 newPosition, GameObject requester)
    {
        if (currentOwner == null || currentOwner == requester)
        {
            currentOwner = requester;
            Vector2 offset = new Vector2(offsetX, offsetY);
            newPosition += offset;
            transform.position = newPosition;
        }
    }

    public void Destroying()
    {
        Destroy(gameObject);
    } 

    public void BeDropped()
    {
        currentOwner = null;
    }
}
