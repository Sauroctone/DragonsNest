using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

    public Transform target;
    [HideInInspector]
    public Vector3 targetOriginPos;
    Vector3 targetPos;
    float lerp;
    public float followLerp;
    public float shootLerp;
    public float lookRange;
    Vector3 baseOffset;
    public Vector3 fireZoom;
    public Vector3 sprintDezoom;
    public Vector3 slowZoom;
    public Vector3 selfDestructZoom;
    Vector3 zoom;
    Vector3 lastPos;
    float rhinput;
    float rvinput;
    Vector3 lookOffset;
    Coroutine selfDestructCor;

    [Header("References")]
    public PlayerController player;
    public ScreenShakeGenerator shakeGen;

    private void Start()
    {
        player = GameManager.Instance.player;
        target = GameManager.Instance.playerControllerInstance.transform.GetChild(0);
        baseOffset = transform.position;
        targetOriginPos = target.localPosition;
    }

    void FixedUpdate ()
    {
        rhinput = Input.GetAxis("Horizontal_R");
        rvinput = Input.GetAxis("Vertical_R");
        lookOffset = new Vector3(rhinput, 0f, rvinput);

        if (player.isShooting)
        {
            zoom = fireZoom;
            lerp = shootLerp;
        }
        else
        {
            lerp = followLerp;

            if (player.isSprinting && !player.isSlowing)
            {
                zoom = sprintDezoom;
            }
            else if (player.isSlowing && !player.isSprinting)
            {
                zoom = slowZoom;
            }
            else
            {
                zoom = Vector3.zero;
            }
        }

        if (player.selfDestructTime > 0)
        {
            zoom = selfDestructZoom * player.selfDestructTime;
            shakeGen.ShakeScreen(player.selfDestructScrShake);
        }

        targetPos = target.position + baseOffset + zoom + lookOffset * lookRange; 
        transform.position = Vector3.Lerp(transform.position, targetPos , lerp);

        if (player.isShooting)
            shakeGen.ShakeScreen(player.shootScrShake);
    }
}