using System.Collections;
using UnityEngine;

public class GlobalEnemySpawner : MonoBehaviour
{
    [Header("What to Spawn & Cap")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField][Tooltip("The enemy tag to count")] private string enemyCountTag = "Seeker";
    [SerializeField] private int maxEnemies = 10;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private float spawnChance = 0.8f;
    [SerializeField] private bool spawnerEnabled = true;
    [SerializeField] private float minSpawnDistance = 40f;
    [SerializeField] private float maxSpawnDistance = 60f;

    private Transform playerPos;

    void Awake() => playerPos = GameObject.FindGameObjectWithTag("Player").transform;

    void Start() => StartCoroutine(SpawnEnemyCoroutine());

    private IEnumerator SpawnEnemyCoroutine()
    {
        while (spawnerEnabled)
        {
            yield return new WaitForSeconds(spawnInterval);

            // count how many of *this* enemy are alive:
            int currentCount = GameObject.FindGameObjectsWithTag(enemyCountTag).Length;
            if (Random.value <= spawnChance && currentCount < maxEnemies)
            {
                float angle = Random.Range(0f, Mathf.PI * 2f);
                float radius = Random.Range(minSpawnDistance, maxSpawnDistance);
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
    
    #if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (playerPos == null)
        {
            // Try to find the player in the editor if not set
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerPos = player.transform;
        }

        if (playerPos != null)
        {
            // Set colors for the gizmos
            Gizmos.color = new Color(0, 1, 0, 0.3f); // Green, semi-transparent
            Gizmos.DrawWireSphere(playerPos.position, minSpawnDistance);
            Gizmos.color = new Color(1, 0.5f, 0, 0.3f); // Orange, semi-transparent
            Gizmos.DrawWireSphere(playerPos.position, maxSpawnDistance);
        }
    }
#endif
}