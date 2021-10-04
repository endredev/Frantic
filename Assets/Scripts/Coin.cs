using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] [Range(0, 1)] float sfxDeathVolume = 1f;
    [SerializeField] GameObject deathVFX = null;
    [SerializeField] GameObject deathVFXSmoke = null;
    [SerializeField] AudioClip deathSFX = null;

    void FixedUpdate()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.05f, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (player)
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
