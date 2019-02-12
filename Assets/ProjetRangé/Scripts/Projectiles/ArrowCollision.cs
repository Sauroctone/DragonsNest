using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCollision : Projectile {

    public GameObject visualCounterpart;

    private void OnDestroy()
    {
        if (visualCounterpart != null)
            Destroy(visualCounterpart);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (destroyOnContact)
            Destroy(gameObject);
    }
}
