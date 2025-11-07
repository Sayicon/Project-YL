using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EnemySpawn
{
    public EnemyConfigSO enemyConfig;
    [Range(0f, 1f)] public float spawnChance;
}

[System.Serializable]
public class Wave
{
    public string waveName;
    public float duration;
    public List<EnemySpawn> enemySpawns;
    public float spawnInterval;
}

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    [Header("Wave Configuration")]
    [SerializeField] private List<Wave> waves;
    [SerializeField] private int maxConcurrentEnemies = 50;

    [Header("Spawn Configuration")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float minSpawnRadius = 10f;
    [SerializeField] private float maxSpawnRadius = 20f;
    [SerializeField, Range(0f, 1f)] private float eliteChance = 0.1f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float minSpawnClearanceRadius = 1.5f;


    private int currentWaveIndex = 0;
    private float waveTimer;
    private float spawnTimer;
    private Transform playerTransform;
    private List<GameObject> activeEnemies = new List<GameObject>();
    private Dictionary<EnemyConfigSO, List<GameObject>> enemyPool = new Dictionary<EnemyConfigSO, List<GameObject>>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        Creature.OnCreatureDied += HandleCreatureDeath;
    }

    private void OnDestroy()
    {
        Creature.OnCreatureDied -= HandleCreatureDeath;
    }

    private void HandleCreatureDeath(GameObject creature)
    {
        if (activeEnemies.Contains(creature))
        {
            activeEnemies.Remove(creature);
        }
    }

    private void Update()
    {
        if (currentWaveIndex >= waves.Count) return; // All waves completed

        waveTimer += Time.deltaTime;
        spawnTimer += Time.deltaTime;

        Wave currentWave = waves[currentWaveIndex];

        if (spawnTimer >= currentWave.spawnInterval)
        {
            spawnTimer = 0f;
            if (activeEnemies.Count < maxConcurrentEnemies)
            {
                SpawnEnemy(currentWave);
            }
        }

        if (waveTimer >= currentWave.duration)
        {
            waveTimer = 0f;
            currentWaveIndex++;
        }
    }

    private void SpawnEnemy(Wave wave)
    {
        Vector3 spawnPosition = GetSpawnPosition();
        if (spawnPosition == Vector3.zero) return; // No valid spawn position found

        EnemyConfigSO selectedEnemyConfig = GetRandomEnemy(wave);
        if (selectedEnemyConfig == null) return;

        GameObject enemyObject = GetEnemyFromPool(selectedEnemyConfig);
        if (enemyObject == null) return;

        enemyObject.transform.position = spawnPosition;
        enemyObject.transform.rotation = Quaternion.identity;
        enemyObject.SetActive(true);

        // Fizik motorunu resetle
        Rigidbody rb = enemyObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }


        Enemy enemy = enemyObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            bool isElite = Random.value < eliteChance;
            enemy.SetElite(isElite);
            enemy.InitEnemyConfig(selectedEnemyConfig);
        }

        activeEnemies.Add(enemyObject);
    }

    private GameObject GetEnemyFromPool(EnemyConfigSO config)
    {
        if (enemyPool.ContainsKey(config))
        {
            foreach (var enemy in enemyPool[config])
            {
                if (!enemy.activeInHierarchy)
                {
                    return enemy;
                }
            }
        }

        GameObject newEnemy = Instantiate(enemyPrefab);
        if (!enemyPool.ContainsKey(config))
        {
            enemyPool[config] = new List<GameObject>();
        }
        enemyPool[config].Add(newEnemy);
        return newEnemy;
    }

    private EnemyConfigSO GetRandomEnemy(Wave wave)
    {
        float totalChance = 0f;
        foreach (var enemySpawn in wave.enemySpawns)
        {
            totalChance += enemySpawn.spawnChance;
        }

        float randomValue = Random.Range(0, totalChance);
        float cumulativeChance = 0f;
        foreach (var enemySpawn in wave.enemySpawns)
        {
            cumulativeChance += enemySpawn.spawnChance;
            if (randomValue <= cumulativeChance)
            {
                return enemySpawn.enemyConfig;
            }
        }

        return null;
    }

    private Vector3 GetSpawnPosition()
    {
        for (int i = 0; i < 30; i++) // Try 30 times to find a valid position
        {
            Vector2 randomCircle = Random.insideUnitCircle.normalized * Random.Range(minSpawnRadius, maxSpawnRadius);
            Vector3 potentialPosition = playerTransform.position + new Vector3(randomCircle.x, 0, randomCircle.y);

            // Raycast down to find the ground
            if (Physics.Raycast(potentialPosition + Vector3.up * 50f, Vector3.down, out RaycastHit hit, 100f, groundLayer))
            {
                // Check for obstacles at the spawn point and other enemies
                if (!Physics.CheckSphere(hit.point, minSpawnClearanceRadius, obstacleLayer))
                {
                    // Also check if there are any active enemies too close
                    bool tooCloseToOtherEnemies = false;
                    foreach (GameObject enemy in activeEnemies)
                    {
                        if (enemy.activeSelf && Vector3.Distance(hit.point, enemy.transform.position) < minSpawnClearanceRadius)
                        {
                            tooCloseToOtherEnemies = true;
                            break;
                        }
                    }

                    if (!tooCloseToOtherEnemies)
                    {
                        return hit.point;
                    }
                }
            }
        }

        return Vector3.zero; // Return zero vector if no valid position is found
    }
}
