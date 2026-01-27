using UnityEngine;

public class shootingtitle : MonoBehaviour
{
    [SerializeField] public GameObject[] toTurnOn;

    public void TurnOff()
    {
        foreach (GameObject go in toTurnOn)
        {
            go.SetActive(true);
        }

        this.gameObject.SetActive(false);
    }
}
