using System.Collections;
using UnityEngine;

public class GlobalEnemySpawner : MonoBehaviour
{
    [SerializeField] private bool spawnerEnabled = true;

    [Header("What to Spawn")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField][Tooltip("The enemy tag to count")] private string enemyCountTag = "Seeker";


    [Header("Base Spawn Settings")]
    [SerializeField] private int baseMaxEnemies = 10;
    [SerializeField] private float baseSpawnInterval = 1.5f;
    [SerializeField] private float baseSpawnChance = 0.8f;
    [SerializeField] private float baseMinSpawnDistance = 40f;
    [SerializeField] private float baseMaxSpawnDistance = 60f;

    [Header("Scaling Toggles")]
    [SerializeField] private bool scaleMaxEnemies         = true;
    [SerializeField] private bool scaleSpawnInterval      = true;
    [SerializeField] private bool scaleSpawnChance        = true;
    [SerializeField] private bool scaleMinSpawnDistance   = false;
    [SerializeField] private bool scaleMaxSpawnDistance   = false;

    [Header("Interval Decay")]
    [SerializeField][Tooltip("How quickly spawnInterval shrinks per difficulty point")] private float intervalDecayRate = 0.2f;
    [SerializeField][Tooltip("Minimum spawn interval after decay")]    private float minSpawnInterval   = 0.3f;


    [Header("Runtime (calculated)")]
    [SerializeField] private int   maxEnemies;
    [SerializeField] private float spawnInterval;
    [SerializeField] private float spawnChance;
    [SerializeField] private float minSpawnDistance;
    [SerializeField] private float maxSpawnDistance;

    private Transform playerPos;

    void Awake()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;

        
    }

    void Start()
    {
        StartCoroutine(SpawnEnemyCoroutine());
        
        // Difficulty multiplier
        float dm = PilotGameController.Instance.difficultyMultiplier;

        // Scale max enemies
        maxEnemies = scaleMaxEnemies
            ? Mathf.RoundToInt(baseMaxEnemies * dm)
            : baseMaxEnemies;

        // Scale interval with exponential decay + clamp
        if (scaleSpawnInterval)
        {
            float decayed = baseSpawnInterval * Mathf.Exp(-intervalDecayRate * (dm - 1));
            spawnInterval = Mathf.Max(decayed, minSpawnInterval);
        }
        else spawnInterval = baseSpawnInterval;

        // Scale other settings
        spawnChance      = scaleSpawnChance      ? Mathf.Clamp01(baseSpawnChance      * dm) : baseSpawnChance;
        minSpawnDistance = scaleMinSpawnDistance ? baseMinSpawnDistance / dm           : baseMinSpawnDistance;
        maxSpawnDistance = scaleMaxSpawnDistance ? baseMaxSpawnDistance * dm           : baseMaxSpawnDistance;

        // Ensure min distance is non-negative
        minSpawnDistance = Mathf.Max(0, minSpawnDistance);
    }

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