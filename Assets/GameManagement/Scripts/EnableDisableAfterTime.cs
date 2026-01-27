using UnityEngine;

public class EnableDisableAfterTime : MonoBehaviour
{
    [SerializeField] GameObject[] objectsToEnable;
    [SerializeField] GameObject[] objectsToDisable;
    [SerializeField] float delayInSeconds = 5f;

    bool done = false;

    void Update()
    {
        if (done) return;
        
        delayInSeconds -= Time.deltaTime;
        if (delayInSeconds <= 0f)
        {
            foreach (GameObject obj in objectsToEnable)
            {
                obj.SetActive(true);
            }

            foreach (GameObject obj in objectsToDisable)
            {
                obj.SetActive(false);
            }
            done = true;
        }
    }
}
