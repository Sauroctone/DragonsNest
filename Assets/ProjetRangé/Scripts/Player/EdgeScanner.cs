using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeScanner : MonoBehaviour {

    public PlayerController player;
    RaycastHit hit;
    public float distanceToScan;
    public LayerMask layerMask;

    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, Vector3.Distance(transform.position, player.shootTarget.position) + distanceToScan, layerMask))
        {
            player.TurnAround(hit.normal);
        }
    }
}
