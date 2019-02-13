using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

    public Transform target;
    internal Vector3 targetOriginPos;
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
    public ScreenShaker shaker;

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
            shaker.SetTrauma(2f, .5f, 15f, .5f);
        }

        targetPos = target.position + baseOffset + zoom + lookOffset * lookRange; 
        transform.position = Vector3.Lerp(transform.position, targetPos , lerp);

        if (player.isShooting)
            shaker.SetTrauma(2f, .3f, 15f, .8f);
    }
}