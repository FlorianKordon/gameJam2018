using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// see https://www.youtube.com/watch?v=9A9yj8KnM8c
public class CameraShake : MonoBehaviour
{
    public float shakeRange = 1f;

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 origPos = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-shakeRange, shakeRange) * magnitude;
            float y = Random.Range(-shakeRange, shakeRange) * magnitude;

            transform.localPosition = new Vector3(x, y, origPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = origPos;
    }
}
