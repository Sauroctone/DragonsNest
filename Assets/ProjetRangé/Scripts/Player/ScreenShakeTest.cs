using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeTest : MonoBehaviour {

    float shakeTimer;
    float shakeSize;
    float shakeSpeed;
    Coroutine shakeCor;

    public float debugShakeTimer;
    public float debugShakeSize;
    public float debugShakeSpeed;

    public void ShakeScreenFor(float _shakeTmr, float _shakeSize, float _shakeSpeed)
    {
        shakeTimer = _shakeTmr;
        shakeSize = _shakeSize;
        shakeSpeed = _shakeSpeed;

        if (shakeCor != null)
            StopCoroutine(shakeCor);
        shakeCor = StartCoroutine(ShakeCor());
    }

    IEnumerator ShakeCor()
    {
        Vector2 insideUnitCircle = Vector2.zero;
        Vector3 shakeVector = Vector3.zero;
        while (shakeTimer > 0)
        {
            if (Vector3.Distance(transform.localPosition, shakeVector) > 0.1)
                transform.localPosition = Vector3.Lerp(Vector3.zero, shakeVector, shakeSpeed * Time.deltaTime);
            else
            {
                insideUnitCircle = Random.insideUnitCircle * shakeSize;
                shakeVector = new Vector3(insideUnitCircle.x, insideUnitCircle.x, 0);
            }
            shakeTimer -= Time.deltaTime;
            yield return null;
        }
        while (Vector3.Distance(transform.localPosition, Vector3.zero) > 0.1)
        {
            transform.localPosition = Vector3.Lerp(Vector3.zero, shakeVector, shakeSpeed * Time.deltaTime);
            yield return null;
        }
        transform.localPosition = Vector3.zero;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.RightShift))
        //{
        //    ShakeScreenFor(debugShakeTimer, debugShakeSize, debugShakeSpeed);
        //}
    }
}