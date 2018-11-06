using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFireCollision : Projectile {

    void OnParticleSystemStopped()
    {
        Destroy(gameObject);
    }
}
