using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] spawnSpots;
    public GameObject[] enemies;
    private int randomSpawnSpot;
    private int innerScore;
    private int enemyLevel = 0;
    private float staticMinSpawnTime = 0.8f;
    private float minSpawnTime = 1f;
    private float staticMaxSpawnTime = 1.5f;
    private float maxSpawnTime = 3f;
    bool enemiesStartedSpawn = false;

    [SerializeField] bool looping = false;

    private void Update()
    {
        if (!enemiesStartedSpawn)
        {
            StartCoroutine(checkForSpawningEnemies());
        }
    }

    private IEnumerator checkForSpawningEnemies()
    {
        if (GameSession.GetGameStarted())
        {
            enemiesStartedSpawn = true;
            yield return StartCoroutine(SpawnEnemies());
        }
    }

    private IEnumerator SpawnEnemies()
    {
        while (looping)
        {
            EnemyBoostMechanicCalculator();

            float toSpawn = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(toSpawn);
            randomSpawnSpot = Random.Range(0, spawnSpots.Length);
            int indexToPick = Random.Range(0, enemyLevel);
            Instantiate(enemies[indexToPick], spawnSpots[randomSpawnSpot].position, Quaternion.identity);
            innerScore++;
        }
    }

    private void EnemyBoostMechanicCalculator()
    {
        /* Boost mechanic for AI */
        if (innerScore >= 35)
        {
            innerScore = 0;
            minSpawnTime *= 0.4f;
            maxSpawnTime *= 0.2f;

            if (minSpawnTime <= 0.6)
            {
                minSpawnTime = staticMinSpawnTime;
                maxSpawnTime = staticMaxSpawnTime;
                enemyLevel++;
            }
        }
    }
}