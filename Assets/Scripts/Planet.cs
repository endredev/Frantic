using UnityEngine;

public class Planet : MonoBehaviour
{
    void FixedUpdate()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - UnityEngine.Random.Range(0.01f, 0.05f), gameObject.transform.position.z);
    }
}
