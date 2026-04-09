using UnityEngine;

public class ArrowBullet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float lifeTime = 5f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * Time.deltaTime * 10f);

        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            lifeTime -= .01f;
        }
    }
}
