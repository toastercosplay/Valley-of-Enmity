using UnityEngine;
using UnityEngine.Events;

public class ForTheClock : MonoBehaviour
{
    [SerializeField] UnityEvent onTimeEnd;

    void EndIt()
    {
        
        //Debug.Log("ForTheClock: Time's up!");
        onTimeEnd.Invoke();
    }
}
