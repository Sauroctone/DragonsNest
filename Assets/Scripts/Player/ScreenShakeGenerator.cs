using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeGenerator : MonoBehaviour {

    float shakeTimer;
    float shakeAmount;
    Vector3 originPos;
    Coroutine shakeCor;

    void Start()
    {
        originPos = transform.position;
    }

    public void ShakeScreen(float shakeTmr, float shakeAmt)
    {
        shakeTimer = shakeTmr;
        shakeAmount = shakeAmt;
        shakeCor = StartCoroutine(ShakeCor());
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

       // transform.position = originPos;
    }
}
