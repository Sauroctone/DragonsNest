using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyDragonPickup : MonoBehaviour {

    public BabyDragonManager babyDragonMan;
    public PlayerController pc;

    private void Start()
    {
        babyDragonMan = pc.babyDragonMan;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Egg"))
        {
            if (babyDragonMan.babyDragons.Count < babyDragonMan.maxBabyDragonCount)
            {

                babyDragonMan.SpawnNewBabyDragon();
                other.GetComponentInParent<Egg>().Die();
            }
        }           
    }
}
