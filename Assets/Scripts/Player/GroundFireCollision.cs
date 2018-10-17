using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFireCollision : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Archer")
        {
            Destroy(other.gameObject);
        }
    }
}
