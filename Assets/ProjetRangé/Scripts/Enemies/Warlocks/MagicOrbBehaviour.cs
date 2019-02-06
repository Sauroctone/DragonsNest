using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicOrbBehaviour : ArrowBehaviour {

    public Rigidbody rb;

    public override void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public override void Init(float _lifetime, Vector3 _aimDir, float _arrowSpeed, Transform _target, AnimationCurve _visualTrajectory)
    {
        lifetime = _lifetime;
        velocity = _aimDir * _arrowSpeed;
        rb.velocity = velocity;
        transform.rotation = Quaternion.LookRotation(velocity.normalized);
    }

    public override void Update()
    { 
        //
    }
}
