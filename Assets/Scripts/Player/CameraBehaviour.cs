using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

    public Transform target;
    Vector3 targetPos;
    public float followLerp;
    public float aimOffset;
    Vector3 baseOffset;
    public Vector3 fireZoom;
    public Vector3 sprintDezoom;
    public Vector3 slowZoom;
    Vector3 zoom;
    Vector3 lastPos;

    [Header("References")]
    public PlayerController player;
    public ScreenShakeGenerator shakeGen;

    private void Start()
    {
        baseOffset = transform.position;
    }

    void FixedUpdate ()
    {
        if (player.isShooting)
            zoom = fireZoom;
        else if (player.isSprinting && !player.isSlowing)
            zoom = sprintDezoom;
        else if (player.isSlowing && !player.isSprinting)
            zoom = slowZoom;
        else
            zoom = Vector3.zero;

        targetPos = target.position + zoom + target.forward * aimOffset + baseOffset;
        transform.position = Vector3.Lerp(transform.position, targetPos , followLerp);

        if (player.isShooting)
            shakeGen.ShakeScreen(player.scrShakeAmount);
    }
}