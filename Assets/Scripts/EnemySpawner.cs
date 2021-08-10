using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] spawnSpots;
    public GameObject enemy;
    private int randomSpawnSpot;
    private int innerScore;
    private float minSpawnTime = 2f;
    private float maxSpawnTime = 4f;
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
            if (innerScore >= 10)
            {
                innerScore = 0;
                minSpawnTime *= 0.8f;
                maxSpawnTime *= 0.8f;
            }

            float toSpawn = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(toSpawn);
            innerScore++;
            randomSpawnSpot = Random.Range(0, spawnSpots.Length);
            Instantiate(enemy, spawnSpots[randomSpawnSpot].position, Quaternion.identity);
        }
    }
}