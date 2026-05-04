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
    private float nextFireTime;

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
            float timeToWait = nextFireTime - Time.time;

            if (timeToWait > 0)
            {
                yield return new WaitForSeconds(timeToWait);
            }

            Instantiate(bulletPrefab, transform.position, transform.rotation);

            nextFireTime = Time.time + shootDelay;

            yield return new WaitForSeconds(shootDelay);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void Update()
    {
        if (moveInput != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
        }
    }

    
}
