using UnityEngine;
using System.Collections;

public class Dragon : MonoBehaviour
{
    [SerializeField] float attackDelay = 5f;
    [SerializeField] float minX = -60f;
    [SerializeField] float maxX = 60f;

    Animator anim;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackDelay);
            transform.position = new Vector3(Random.Range(minX, maxX), transform.position.y, transform.position.z);
            anim.SetTrigger("Attack");
        }
    }
}
