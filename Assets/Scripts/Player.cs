using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /* Configuration params */

    [Header("Player Movement")]
    [SerializeField] float moveSpeed = 14f;
    [SerializeField] float padding = 0.2f;

    [Header("Projectile")]
    [SerializeField] int health = 100;
    [SerializeField] int maxHealth = 100;
    [SerializeField] float projectileSpeed = 100f;    
    [SerializeField] float durationOfExplosion = 1f;
    [SerializeField] float durationOfLaserShot = 0.5f;
    [SerializeField] float shootCoolDown = .5f;
    [SerializeField] [Range(0, 1)] float sfxFireVolume = 1f;
    [SerializeField] [Range(0, 1)] float sfxDeathVolume = 1f;
    [SerializeField] [Range(0, 10)] float sfxShieldHitVolume = 3f;
    [SerializeField] GameObject Ship = null;
    [SerializeField] GameObject[] AvailableShips = null;
    [SerializeField] GameObject deathVFX = null;
    [SerializeField] GameObject shootVFX = null;
    [SerializeField] GameObject laserPrefab = null;
    [SerializeField] GameObject shield = null;
    [SerializeField] GameObject laserShotFrom = null;
    [SerializeField] AudioClip deathSFX = null;
    [SerializeField] AudioClip shieldHitSFX = null;
    [SerializeField] AudioClip shootSFX = null;
    [SerializeField] HealthBar healthBar = null;

    private float xMin;
    private float yMin;
    private float xMax;
    private float yMax;
    private Boolean canShoot = true;

    public Joystick joystick;
    public CameraShake cameraShake;

    // Start is called before the first frame update
    void Start()
    {
        healthBar.setMaxHealth(health);
        SetupMoveBoundaries();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ManualFire();
        }
    }

    public void ManualFire()
    {
        if (canShoot)
        {
            canShoot = false;
            AudioSource.PlayClipAtPoint(shootSFX, Camera.main.transform.position, sfxFireVolume);
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);

            GameObject laserShot = Instantiate(shootVFX, laserShotFrom.transform.position, transform.rotation);
            Destroy(laserShot, durationOfLaserShot);

            StartCoroutine(fireCoolDown());
        }
    }

    IEnumerator fireCoolDown()
    {
        yield return new WaitForSeconds(shootCoolDown);
        canShoot = true;
    }

    private void SetupMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 0.5f, 0)).y - padding;
    }

    public void SetHealth(int amount)
    {
        health = amount;
    }

    public void IncreaseHealth(int amount)
    {
        int missingMaxHealth = maxHealth - health;
        if (amount > missingMaxHealth)
        {
            amount = missingMaxHealth;
        }
        health += amount;
        healthBar.SetHealth(health);
    }

    public void IncreaseFireRate(float percentage)
    {
        shootCoolDown = shootCoolDown - (shootCoolDown * (percentage / 100));
    }

    private void Move()
    {
        var deltaX = joystick.Horizontal * Time.deltaTime * moveSpeed;
        var deltaY = joystick.Vertical * Time.deltaTime * moveSpeed;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        transform.position = new Vector2(newXPos, newYPos);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();

        if(!damageDealer) { return; }
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        healthBar.SetHealth(health);
        damageDealer.Hit();
        StartCoroutine(cameraShake.Shake(.2f, .1f));

        if (health <= 0)
        {
            Die();
        } else
        {
            shield.SetActive(true);
            SpriteRenderer shieldRenderer = shield.GetComponent<SpriteRenderer>();
            shieldRenderer.color = new Color(shieldRenderer.color.r, shieldRenderer.color.g, shieldRenderer.color.b, 1);
            StartCoroutine(FadeAlphaToZero(shieldRenderer, 0.5f));
            AudioSource.PlayClipAtPoint(shieldHitSFX, Camera.main.transform.position, sfxShieldHitVolume);
        }
    }

    private void Die()
    {
        FindObjectOfType<Level>().LoadGameOver();
        Destroy(gameObject);
        GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(explosion, durationOfExplosion);
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, sfxDeathVolume);
    }

    public float getHelath()
    {
        return health;
    }

    IEnumerator FadeAlphaToZero(SpriteRenderer renderer, float duration)
    {
        Color startColor = renderer.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0);
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            renderer.color = Color.Lerp(startColor, endColor, time / duration);
            yield return null;
        }

        shield.SetActive(false);
    }
}
