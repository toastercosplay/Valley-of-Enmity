using UnityEngine;
using UnityEngine.InputSystem;

public class test : MonoBehaviour
{
    PlayerInput playerInput;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        foreach (var device in playerInput.devices)
        {
            //Debug.Log("Player device: " + device.displayName + " : " + device.layout);
        }
    }
}
