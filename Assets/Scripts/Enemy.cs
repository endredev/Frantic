using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("Enemy Stats")]
    [SerializeField] float health = 100;
    [SerializeField] int scoreValue = 100;
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 2f;
    [SerializeField] float projectileSpeed = 1f;
    [SerializeField] float durationOfExplosion = 1f;
    [SerializeField] [Range(0, 1)]  float sfxFireVolume = 1f;
    [SerializeField] [Range(0, 1)]  float sfxDeathVolume = 1f;
    [SerializeField] GameObject laserPrefab = null;

    [Header("Sound Effects")]
    [SerializeField] GameObject deathVFX = null;
    [SerializeField] AudioClip deathSFX = null;
    [SerializeField] AudioClip fireSFX = null;
    [SerializeField] Boolean shouldMove = true;

    // Start is called before the first frame update
    void Start()
    {
        if (shouldMove)
        {
            StartMoving();
        }
        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);

    }

    private void StartMoving()
    {
        GameObject[] endPoints1 = GameObject.FindGameObjectsWithTag("EndPoint1");
        GameObject[] endPoints2 = GameObject.FindGameObjectsWithTag("EndPoint2");
        GameObject[] despawnPoints = GameObject.FindGameObjectsWithTag("DespawnPoint");

        int rand1 = UnityEngine.Random.Range(0, endPoints1.Length);
        int rand2 = UnityEngine.Random.Range(0, endPoints2.Length);
        int rand3 = UnityEngine.Random.Range(0, despawnPoints.Length);

        Vector3 startPos = gameObject.transform.position;
        Vector3[] path = new Vector3[] { startPos, endPoints1[rand1].transform.position, endPoints2[rand2].transform.position, despawnPoints[rand3].transform.position };
        LeanTween.moveLocal(gameObject, path, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;

        if(shotCounter <=0)
        {
            Fire();
            shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        var FixedPosition = new Vector2(transform.position.x, transform.position.y - 1);
        GameObject laser = Instantiate(laserPrefab, FixedPosition, Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(fireSFX, Camera.main.transform.position, sfxFireVolume);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (damageDealer)
        {
            ProcessHit(damageDealer);
        }
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    public void GetDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        FindObjectOfType<GameSession>().AddScore(scoreValue);
        Destroy(gameObject);
        GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(explosion, durationOfExplosion);
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, sfxDeathVolume);
    }
}
