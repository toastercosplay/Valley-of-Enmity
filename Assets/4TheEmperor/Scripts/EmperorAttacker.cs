using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.InputSystem;

public class EmperorAttacker : MonoBehaviour
{
    [SerializeField] private GameObject unitPrefab;
    [SerializeField] private SplineContainer path;

    public float minSize = 0.5f, maxSize = 3f, growthSpeed = 2f;
    public float minMoveSpeed = 1f, maxMoveSpeed = 6f;

    private Unit currentUnit;

    public void OnSpawn(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //Debug.Log("Spawning unit...");
            
            
            GameObject go = Instantiate(unitPrefab);
            currentUnit = go.GetComponent<Unit>();
            
            // Pass all settings to the unit immediately
            currentUnit.StartCharging(path, minSize, maxSize, growthSpeed, minMoveSpeed, maxMoveSpeed);
        }

        if (context.canceled && currentUnit != null)
        {
            Debug.Log("Releasing unit with final size: " + currentUnit.transform.localScale.x);
            
            currentUnit.Release();
            currentUnit = null;
        }
    }
}
