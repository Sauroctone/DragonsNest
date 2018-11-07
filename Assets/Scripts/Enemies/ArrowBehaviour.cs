﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviour : MonoBehaviour {

    float lifetime;
    public Rigidbody visualRb;
    public Rigidbody colliderRb;
<<<<<<< HEAD
=======
    Vector3 velocity;
    float arrowMaxHeight;
    Vector3 lastPos;
>>>>>>> master

    public void Init(ArcherBehaviour _owner)
    {
        lifetime = 3; //temp

<<<<<<< HEAD
        visualRb.velocity = _owner.aimDir * _owner.group.arrowSpeed;
=======
        //Visual
        velocity = Vector3.ProjectOnPlane(_owner.aimDir, Vector3.up) * _owner.group.arrowSpeed;
        visualRb.velocity = velocity;
        arrowMaxHeight = _owner.group.arrowMaxHeight;
        visualCurve = _owner.group.visualTrajectory;
        transform.rotation = Quaternion.LookRotation(velocity.normalized);

        //Collider
>>>>>>> master
        colliderRb.transform.parent = null;
        colliderRb.transform.position = new Vector3(colliderRb.transform.position.x, GameManager.Instance.player.transform.position.y, colliderRb.transform.position.z);
        Vector3 colliderDir = Vector3.ProjectOnPlane(_owner.aimDir, Vector3.up);
        colliderRb.transform.rotation = Quaternion.LookRotation(colliderDir);
        colliderRb.velocity = colliderDir * _owner.group.arrowSpeed;
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
        Destroy(colliderRb.gameObject, lifetime);
    }
<<<<<<< HEAD
}
=======

    private void Update()
    {
        float lastFrame = time;
        time += Time.deltaTime;
        float newFrame = time;
        transform.position = new Vector3(transform.position.x, visualCurve.Evaluate(time/lifetime) * arrowMaxHeight, transform.position.z);
        float o = visualCurve.Evaluate(newFrame/lifetime) - visualCurve.Evaluate(lastFrame/lifetime);
        float a = newFrame/lifetime - lastFrame/lifetime;
        float angle = Mathf.Atan2(o, a) * Mathf.Rad2Deg;
        //Debug.Log(angle);
        if (visualRb != null)
            visualRb.transform.localEulerAngles = new Vector3(-angle / 3, 0, 0);
    }
}
>>>>>>> master
