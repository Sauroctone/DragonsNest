using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AncientProjectileCollision : MonoBehaviour {

    public Transform explosion;
    public Transform explosionVisuals;

    private void OnTriggerEnter(Collider other)
    {
        explosion.gameObject.SetActive(true);
        explosion.parent = null;
        explosionVisuals.gameObject.SetActive(true);
        explosionVisuals.parent = null;
        Destroy(gameObject);
    }
}