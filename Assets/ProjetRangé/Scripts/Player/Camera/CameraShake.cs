using UnityEngine;
using System.Collections;


public class CameraShake : MonoBehaviour
{
    private Vector3 originalCamPos;

    private bool shaking;

    void Start()
    {
        originalCamPos = transform.position;
    }

    public IEnumerator Shake(float _magnitude, float _duration)
    {
        float elapsed = 0.0f;
        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;

            float percentComplete = elapsed / _duration;
            float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

            // map value to [-1, 1]
            float x = Random.value * 2.0f - 1.0f;
            float y = Random.value * 2.0f - 1.0f;
            x *= _magnitude * damper;
            y *= _magnitude * damper;

            transform.position = new Vector3(originalCamPos.x + x, originalCamPos.y + y, originalCamPos.z);

            yield return null;
        }
        transform.position = originalCamPos;
    }

    public void StartContinousShake(float _magnitude, float _duration)
    {
        StopAllCoroutines();

        StartCoroutine(ContinuousShake(_magnitude, _duration));
    }

    IEnumerator ContinuousShake(float _magnitude, float _duration)
    {
        shaking = true;

        while (shaking)
        {
            float damper = 1.0f - Mathf.Clamp(4.0f - 3.0f, 0.0f, 1.0f);

            // map value to [-1, 1]
            float x = Random.value * 2.0f - 1.0f;
            float y = Random.value * 2.0f - 1.0f;
            x *= _magnitude * damper;
            y *= _magnitude * damper;

            transform.position = new Vector3(originalCamPos.x + x, originalCamPos.y + y, originalCamPos.z);

            yield return null;
        }

        //End shaking in _duration seconds

        float elapsed = 0.0f;
        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;

            float percentComplete = elapsed / _duration;
            float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

            // map value to [-1, 1]
            float x = Random.value * 2.0f - 1.0f;
            float y = Random.value * 2.0f - 1.0f;
            x *= _magnitude * damper;
            y *= _magnitude * damper;

            transform.position = new Vector3(originalCamPos.x + x, originalCamPos.y + y, originalCamPos.z);

            yield return null;
        }

        transform.position = originalCamPos;
    }

    public void StopShaking()
    {
        shaking = false;
    }

}