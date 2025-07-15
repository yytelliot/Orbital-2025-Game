using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private float spawnInterval = 1.5f;

    [SerializeField]
    private float spawnChance = 0.8f;
    [SerializeField]
    private bool spawnerEnabled = true;
    [SerializeField]
    private float minSpawnDistance = 40f;
    [SerializeField]
    private float maxSpawnDistance = 60f;



    private Coroutine spawnEnemyCoroutine;
    private Transform playerPos;

    void Awake()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        spawnEnemyCoroutine = StartCoroutine(SpawnEnemyCoroutine());
    }

    // Update is called once per frame
    private IEnumerator SpawnEnemyCoroutine()
    {
        while (spawnerEnabled)
        {
            yield return new WaitForSeconds(spawnInterval);
            if (Random.value <= spawnChance)
            {
            // pick a random angle
            float angle    = Random.Range(0f, Mathf.PI * 2f);
            // pick a random radius between min and max
            float radius   = Random.Range(minSpawnDistance, maxSpawnDistance);
            // convert to cartesian
            Vector3 offset = new Vector3(
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius,
                0f
            );

            Instantiate(enemyPrefab,
                        playerPos.position + offset,
                        Quaternion.identity);
        }
        }
        
    }
}
