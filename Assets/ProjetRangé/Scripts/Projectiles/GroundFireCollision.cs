using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFireCollision : Projectile {

    void Start()
    {
        isFire = true;
    }

    void OnParticleSystemStopped()
    {
        Destroy(gameObject);
    }
}
