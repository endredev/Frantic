using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroller : MonoBehaviour
{

    [SerializeField] float backGroundScrollSpeed = 0.02f;
    [SerializeField] float planetSpawnMinTime;
    [SerializeField] Material warpMaterial;
    [SerializeField] GameObject planet;
    [SerializeField] GameObject spawnPointStart;
    [SerializeField] GameObject spawnPointEnd;

    Material myMaterial;
    Vector2 offset;
    bool planetSpawned = false;
    GameObject planetInstantiated;

    // Start is called before the first frame update
    void Start()
    {
        myMaterial = GetComponent<Renderer>().material;
        offset = new Vector2(0, backGroundScrollSpeed);
        planetSpawnMinTime = Random.Range(1f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        myMaterial.mainTextureOffset += offset * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        planetSpawnMinTime -= Time.deltaTime;
        if (!planetSpawned && planetSpawnMinTime <= 0f)
        {
            float xToSpawnOn = Random.Range(spawnPointStart.transform.position.x, spawnPointEnd.transform.position.x);
            planetInstantiated = Instantiate(planet, new Vector3(xToSpawnOn, spawnPointStart.transform.position.y, spawnPointStart.transform.position.z), Quaternion.identity);
            planetSpawnMinTime = Random.Range(20f, 40f);
            planetSpawned = true;
        }

        if (planetSpawned)
        {
            planetInstantiated.transform.position = new Vector3(planetInstantiated.transform.position.x, planetInstantiated.transform.position.y - 0.1f, 0.3f);
        }
    }
}
