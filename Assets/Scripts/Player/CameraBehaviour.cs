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
        else if (player.isSprinting)
            zoom = sprintDezoom;
        else
            zoom = Vector3.zero;

        targetPos = target.position + zoom + target.forward * aimOffset + baseOffset;
        transform.position = Vector3.Lerp(transform.position, targetPos , followLerp);

        if (player.isShooting)
            shakeGen.ShakeScreen(.1f, .075f);
    }
}