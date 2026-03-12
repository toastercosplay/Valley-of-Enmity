using UnityEngine;
using UnityEngine.InputSystem;

public class test : MonoBehaviour
{
    void Start()
    {
        foreach (var device in InputSystem.devices)
        {
            Debug.Log(device.displayName + " : " + device.layout);
        }
    }
}
