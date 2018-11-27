using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AncientDetection : MonoBehaviour {

    public Ancient ancient;

    void OnTriggerEnter(Collider col)
    {
        foreach (EnemyBehaviour _enemy in ancient.targets)
        {
            if (_enemy.transform == col.transform.parent)
                return;
        }

        EnemyBehaviour enemy = col.GetComponentInParent<EnemyBehaviour>();
        if (enemy != null)
            ancient.AddToTargetList(enemy);
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag != "Archer")
            return;

        ancient.RemoveAndChangeTarget(col.GetComponent<EnemyBehaviour>());
    }
}
