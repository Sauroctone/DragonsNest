using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollision : MonoBehaviour {

    public GameObject visualCounterpart;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Dragon")
        {
            print("touché");
            Destroy(gameObject);
            Destroy(visualCounterpart);
        }
    }
}
