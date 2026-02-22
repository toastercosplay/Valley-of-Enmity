using UnityEngine;
using System.Collections;
public class TowerPlayer : MonoBehaviour
{
    [SerializeField] GameObject[] prefabList;

    [SerializeField] float spawnDelay = 1f;
    [SerializeField] float spawnAmount = 3f;
    
    void Start()
    {
         StartCoroutine(SpawnPrefab());
    }

    IEnumerator SpawnPrefab()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);
            int index = Random.Range(0, prefabList.Length);
            for (int i = 0; i < spawnAmount; i++)
            {
                Instantiate(prefabList[index], transform.position + new Vector3(i * 2f, 0f, 0f), Quaternion.identity);
            }
        }
    }
}
