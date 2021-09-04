using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake (float duration, float magintude)
    {
        Vector3 originalPosition = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed <= duration)
        {
            float x = Random.Range(-1, 1) * magintude;
            float y = Random.Range(-1, 1) * magintude;

            transform.localPosition = new Vector3(x, y, transform.position.z);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}
