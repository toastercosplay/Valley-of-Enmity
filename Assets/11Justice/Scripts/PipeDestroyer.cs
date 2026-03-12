using UnityEngine;

public class PipeDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only destroy pipes (tag them "Pipe" OR check for PipeMover component)
        if (other.GetComponentInParent<PipeMovement>() != null)
        {
            Destroy(other.transform.root.gameObject);
        }
    }

}
