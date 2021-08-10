using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerOld : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs = new List<WaveConfig>();
    [SerializeField]  int startingWave = 0;
    [SerializeField]  bool looping = false;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        do
        {
            yield return StartCoroutine(SpawnAllWaves());
        } while (looping);
       
    }

    private IEnumerator SpawnAllWaves()
    {
        for(int waveCount = startingWave; waveCount < waveConfigs.Count; waveCount++ )
        {
            var currentWave = waveConfigs[waveCount];
            yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
        }
    }

    private IEnumerator SpawnAllEnemiesInWave(WaveConfig currentWave)
    {
        for (int enemyCount = 0; enemyCount < currentWave.GetNumberOfEnemies(); enemyCount++)
        {
            var newEnemy = Instantiate(
            currentWave.GetEnemyPrefab(),
            currentWave.GetWaypoints()[0].transform.position,
            Quaternion.identity
            );

            newEnemy.GetComponent<EnemyPathing>().setWaveConfig(currentWave);

            yield return new WaitForSeconds(currentWave.GetTimeBetweenSpawns());
        }
    }
}
