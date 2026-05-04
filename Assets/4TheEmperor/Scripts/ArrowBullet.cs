using UnityEngine;

public class ArrowBullet : MonoBehaviour
{
    private float lifeTime = 5f;

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
