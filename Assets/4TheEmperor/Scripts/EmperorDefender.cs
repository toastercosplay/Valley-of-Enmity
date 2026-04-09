using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class EmperorDefender : MonoBehaviour
{
    public float rotateSpeed = 10f;
    public float shootDelay = 1f;
    [SerializeField] private GameObject bulletPrefab;
    
    private Vector2 moveInput;
    private Coroutine shootCoroutine;
    private float nextFireTime; // This tracks when we are allowed to shoot next

    // ... Update and OnMove remain the same ...

    public void OnA(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (shootCoroutine == null)
            {
                shootCoroutine = StartCoroutine(Shoot());
            }
        }
        else if (context.canceled)
        {
            if (shootCoroutine != null)
            {
                StopCoroutine(shootCoroutine);
                shootCoroutine = null;
            }
        }
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            // 1. Check if enough time has passed since the last shot
            float timeToWait = nextFireTime - Time.time;

            // 2. If we are still in the cooldown period, wait for the remainder
            if (timeToWait > 0)
            {
                yield return new WaitForSeconds(timeToWait);
            }

            // 3. Fire!
            Instantiate(bulletPrefab, transform.position, transform.rotation);

            // 4. Set the time for the NEXT allowed shot
            nextFireTime = Time.time + shootDelay;

            // 5. Wait for the delay for the automatic rapid-fire
            yield return new WaitForSeconds(shootDelay);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        //lerp rotation to the value of the stick
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, -context.ReadValue<Vector2>().x * rotateSpeed), Time.deltaTime * rotateSpeed);
        moveInput = context.ReadValue<Vector2>();
    }

    void Update()
    {
        // Rotate based on moveInput
        if (moveInput != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
        }
    }

    
}
