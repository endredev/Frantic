using UnityEngine;
using System.Linq;

public class BackGroundScroller : MonoBehaviour
{

    [SerializeField] float backGroundScrollSpeed = 15f;
    [SerializeField] float planetSpawnMinTime;
    [SerializeField] GameObject[] planets = null;
    [SerializeField] GameObject spawnPointStart = null;
    [SerializeField] GameObject spawnPointEnd = null;

    Material myMaterial;
    Vector2 offset;
    bool planetSpawned = false;
    int lastSelectedPlanetIndex = 100;
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
        if (GameSession.GetGameStarted()) {
            myMaterial.mainTextureOffset += offset * Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (GameSession.GetGameStarted())
        {
            planetSpawnMinTime -= Time.deltaTime;
            if (!planetSpawned && planetSpawnMinTime <= 0f)
            {
                float xToSpawnOn = Random.Range(spawnPointStart.transform.position.x, spawnPointEnd.transform.position.x);
                GameObject[] planetsToShow = planets.Where((source, index) => index != lastSelectedPlanetIndex).ToArray();
                int indexToSpawnPlanet = Random.Range(0, planetsToShow.Length);
                lastSelectedPlanetIndex = indexToSpawnPlanet;
                planetInstantiated = Instantiate(planetsToShow[indexToSpawnPlanet], new Vector3(xToSpawnOn, spawnPointStart.transform.position.y, 1), Quaternion.identity);
                planetSpawnMinTime = Random.Range(20f, 40f);
                planetSpawned = true;
            }
        }
    }
}
