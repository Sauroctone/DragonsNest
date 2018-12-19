using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AncientDetection : MonoBehaviour {

    public Ancient ancient;

    void OnTriggerEnter(Collider col)
    {
        if (col.tag != "Enemy")
            return;

        if (Vector3.Angle(transform.forward, (col.transform.position - transform.position).normalized) <= ancient.angleOfVision / 2)
        {
            ancient.AddToTargetList(col.transform);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag != "Enemy")
            return;

        ancient.RemoveAndChangeTarget(col.transform);
    }
}
