using UnityEngine;

public class Collectible : MonoBehaviour
{
    
    [SerializeField] public string itemName = "";

    [SerializeField] float offsetX = 0f;
    [SerializeField] float offsetY = 0f;
    
    public void BePickedUp(Vector2 newPosition)
    {
        Vector2 offset = new Vector2(offsetX, offsetY);
        newPosition += offset;
        transform.position = newPosition;
    }

    public void Destroying()
    {
        Destroy(gameObject);
    } 
}
