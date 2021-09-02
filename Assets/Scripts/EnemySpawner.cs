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
    [SerializeField] bool looping = false;

    private IEnumerator Start()
    {
        yield return StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (looping)
        {
            /* Boost mechanic for AI */
            if (innerScore >= 15)
            {
                innerScore = 0;
                minSpawnTime *= 0.8f;
                maxSpawnTime *= 0.8f;

                if (minSpawnTime <= 0.6)
                {
                    minSpawnTime = staticMinSpawnTime;
                    maxSpawnTime = staticMaxSpawnTime;
                    enemyLevel++;
                }
            }

            float toSpawn = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(toSpawn);
            innerScore++;
            randomSpawnSpot = Random.Range(0, spawnSpots.Length);
            int indexToPick = Random.Range(0, enemyLevel);
            Instantiate(enemies[indexToPick], spawnSpots[randomSpawnSpot].position, Quaternion.identity);
        }
    }
}