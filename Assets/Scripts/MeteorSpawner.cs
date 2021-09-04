using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [SerializeField] GameObject spawnPointStart = null;
    [SerializeField] GameObject spawnPointEnd = null;
    [SerializeField] Meteor meteor = null;
    [SerializeField] float meteorSpawnMinTime;


    // Start is called before the first frame update
    void Start()
    {
        meteorSpawnMinTime = Random.Range(1f, 10f);
    }

    private void FixedUpdate()
    {
        meteorSpawnMinTime -= Time.deltaTime;
        if (meteorSpawnMinTime <= 0f)
        {
            float xToSpawnOn = Random.Range(spawnPointStart.transform.position.x, spawnPointEnd.transform.position.x);
            Instantiate(meteor, new Vector3(xToSpawnOn, spawnPointStart.transform.position.y, 0), Quaternion.identity);
            meteorSpawnMinTime = Random.Range(5f, 10f);
        }
    }
}
