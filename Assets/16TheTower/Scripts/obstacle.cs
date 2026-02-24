using UnityEngine;

public class obstacle : MonoBehaviour
{
    [SerializeField] float minX = -60f;
    [SerializeField] float maxX = 60f;
    [SerializeField] float startY = 50f;
    [SerializeField] float speed = 0f;

    
    void Start()
    {
        transform.position = new Vector3(Random.Range(minX, maxX), startY, transform.position.z);
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime, transform.position.z);
        if (transform.position.y < -50f)
        {
            Destroy(gameObject);
        }
    }


}
