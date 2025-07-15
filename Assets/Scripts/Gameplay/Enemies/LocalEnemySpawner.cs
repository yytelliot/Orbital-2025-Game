using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalEnemySpawner : MonoBehaviour
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
    private float range = 40f;

    private Coroutine spawnEnemyCoroutine;

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
                Vector3 offset = new Vector3(Random.Range(-range, range), Random.Range(-range, range), 0);
                GameObject newEnemy = Instantiate(enemyPrefab,
                        offset,
                        Quaternion.identity);
            }
        }
        
    }
}
