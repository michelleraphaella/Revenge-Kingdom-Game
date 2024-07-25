using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private int _maxEnemyCount = 2; // Maximum number of enemies to spawn

    private int _spawnedEnemyCount = 0; // Counter to track the number of spawned enemies

    void Start()
    {
        // Start spawning enemies immediately
        SpawnEnemy();
    }

    void Update()
    {
        if (_spawnedEnemyCount < _maxEnemyCount) // Check if the limit has been reached
        {
            // Spawn an enemy immediately
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        Instantiate(_enemyPrefab, transform.position, Quaternion.identity);
        _spawnedEnemyCount++; // Increment the counter
    }
}
