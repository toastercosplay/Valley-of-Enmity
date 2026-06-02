using UnityEngine;

public class LoverItem : MonoBehaviour
{
    public string itemName = "";
    public Vector3 offset = Vector3.zero;

    public void AssignToParent(Transform newParent)
    {
        transform.SetParent(newParent);
        transform.localPosition = offset;
        
        // Optional: If you use Rigidbodies, you'd disable physics here
        if (TryGetComponent<Rigidbody>(out var rb)) rb.isKinematic = true;
    }

    public void Detach()
    {
        transform.SetParent(null);
        
        // Optional: Re-enable physics here
        if (TryGetComponent<Rigidbody>(out var rb)) rb.isKinematic = false;
    }
}
