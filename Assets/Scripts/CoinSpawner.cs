using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] GameObject spawnPointStart = null;
    [SerializeField] GameObject spawnPointEnd = null;
    [SerializeField] Coin coin = null;
    [SerializeField] float coinSpawnMinTime;


    // Start is called before the first frame update
    void Start()
    {
        coinSpawnMinTime = Random.Range(1f, 10f);
    }

    private void FixedUpdate()
    {
        coinSpawnMinTime -= Time.deltaTime;
        if (coinSpawnMinTime <= 0f)
        {
            float xToSpawnOn = Random.Range(spawnPointStart.transform.position.x, spawnPointEnd.transform.position.x);
            StartCoroutine(SpawnCoins(Random.Range(3f, 8f), 0.5f, xToSpawnOn));
            coinSpawnMinTime = Random.Range(2f, 5f);
        }
    }

    IEnumerator SpawnCoins(float coinCount, float secondsBetweenSpawns, float xToSpawnOn)
    {
        for (int currentCoin = 0; currentCoin < coinCount; currentCoin++)
        {
            Instantiate(coin, new Vector3(xToSpawnOn, spawnPointStart.transform.position.y, 0), Quaternion.identity);
            yield return new WaitForSeconds(secondsBetweenSpawns);
        }
    }
}
