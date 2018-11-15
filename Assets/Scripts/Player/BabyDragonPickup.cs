using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyDragonPickup : MonoBehaviour {

    BabyDragonManager babyDragonMan;

    private void Start()
    {
        babyDragonMan = GameManager.Instance.babyDragonMan;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Egg")
        {
            if (babyDragonMan.babyDragons.Count < babyDragonMan.maxBabyDragonCount)
            {
                babyDragonMan.SpawnNewBabyDragon();
                other.GetComponentInParent<Egg>().Die();
            }
        }           
    }
}
