using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollision : MonoBehaviour {

    public GameObject visualCounterpart;
    public int firePower = 10; 

	void Start ()
    {
        Destroy(gameObject, 3);
    }

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
