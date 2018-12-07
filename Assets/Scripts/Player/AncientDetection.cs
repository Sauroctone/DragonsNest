using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AncientDetection : MonoBehaviour {

    public Ancient ancient;

    void OnTriggerEnter(Collider col)
    {
        if (col.tag != "Enemy")
            return;

        ancient.AddToTargetList(col.transform);
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag != "Enemy")
            return;

        ancient.RemoveAndChangeTarget();
    }
}
