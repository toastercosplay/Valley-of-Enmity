using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PipeSpawner : MonoBehaviour
{
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private float heightRange = 6f;
    [SerializeField] private GameObject pipePreFab;

    private float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnPipe();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > spawnInterval)
        {
            SpawnPipe();
            timer = 0;
        }

        timer += Time.deltaTime;
    }

    private void SpawnPipe()
    {
        Vector3 spawnPos = transform.position + new Vector3(0, Random.Range(-heightRange, heightRange), 0);
        GameObject pipe = Instantiate(pipePreFab, spawnPos, Quaternion.identity);

        Destroy(pipe,10f);
    }
}
