using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [SerializeField] int health = 50;
    [SerializeField] [Range(0, 1)] float sfxDeathVolume = 1f;
    [SerializeField] GameObject deathVFX = null;
    [SerializeField] GameObject deathVFXSmoke = null;
    [SerializeField] AudioClip deathSFX = null;


    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.05f, gameObject.transform.position.z);
        gameObject.transform.Rotate(Vector3.forward, -45.0f * Time.deltaTime);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        Player player = collision.gameObject.GetComponent<Player>();

        if (!damageDealer)
        {
            if (player)
            {
                health = 0;
                Die();
            }
            return;
        }
        else
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

    private void Die()
    {
        Destroy(gameObject);
        GameObject smoke = Instantiate(deathVFXSmoke, transform.position, transform.rotation);
        GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(explosion, 4f);
        Destroy(smoke, 4f);
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, sfxDeathVolume);
    }
}
