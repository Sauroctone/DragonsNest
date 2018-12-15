using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviour : MonoBehaviour {

    float lifetime;
    float time;
    AnimationCurve visualCurve;
    public Rigidbody visualRb;
    public Rigidbody colliderRb;
    Vector3 velocity;
    float arrowMaxHeight;
    Vector3 lastPos;

    public void Init(float _lifetime, Vector3 _aimDir, float _arrowSpeed, Transform _target, AnimationCurve _visualTrajectory)
    {
        lifetime = _lifetime;

        //Visual
        velocity = Vector3.ProjectOnPlane(_aimDir, Vector3.up) * _arrowSpeed;
        visualRb.velocity = velocity;
        arrowMaxHeight = _target.position.y;
        visualCurve = _visualTrajectory;
        transform.rotation = Quaternion.LookRotation(velocity.normalized);

        //Collider
        colliderRb.transform.parent = null;
        colliderRb.transform.position = new Vector3(colliderRb.transform.position.x, _target.transform.position.y, colliderRb.transform.position.z);
        Vector3 colliderDir = Vector3.ProjectOnPlane(_aimDir, Vector3.up);
        if (colliderDir != Vector3.zero)
            colliderRb.transform.rotation = Quaternion.LookRotation(colliderDir);
        colliderRb.velocity = colliderDir * _arrowSpeed;
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
        Destroy(colliderRb.gameObject, lifetime);
    }

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