using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnTree : MonoBehaviour {

    public ParticleSystem ps;

    private void Start()
    {
        print("j'existe");
    }

    private void OnTriggerEnter(Collider other)
    {
        ps.Play();   
    }

    void OnParticleSystemStopped()
    {
        Destroy(transform.parent);
    }
}
