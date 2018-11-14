using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeGenerator : MonoBehaviour {

    float shakeTimer;
    float shakeAmount;
    Coroutine shakeCor;

    public void ShakeScreenFor(float shakeTmr, float shakeAmt)
    {
        shakeTimer = shakeTmr;
        shakeAmount = shakeAmt;
        if (shakeCor != null)
            StopCoroutine(shakeCor);
        shakeCor = StartCoroutine(ShakeCor());
    }

    public void ShakeScreen(float shakeAmt)
    {
        if (shakeCor != null)
            StopCoroutine(shakeCor);

        shakeAmount = shakeAmt;
        Vector2 shakeVector = Random.insideUnitCircle * shakeAmount;
        transform.position = new Vector3(transform.position.x + shakeVector.x, transform.position.y + shakeVector.y, transform.position.z);
    }

    IEnumerator ShakeCor()
    {
        while (shakeTimer > 0)
        {
            Vector2 shakeVector = Random.insideUnitCircle * shakeAmount;
            transform.position = new Vector3(transform.position.x + shakeVector.x, transform.position.y + shakeVector.y, transform.position.z);
            shakeTimer -= Time.deltaTime;
            yield return null;
        }
    }
}
