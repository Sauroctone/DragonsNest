using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviour : MonoBehaviour {

    float lifetime;
    public Rigidbody visualRb;
    public Rigidbody colliderRb;

    public void Init(ArcherBehaviour _owner)
    {
        lifetime = 3; //temp

        visualRb.velocity = _owner.aimDir * _owner.group.arrowSpeed;
        colliderRb.transform.parent = null;
        colliderRb.transform.position = new Vector3(colliderRb.transform.position.x, GameManager.Instance.player.transform.position.y, colliderRb.transform.position.z);
        Vector3 colliderDir = Vector3.ProjectOnPlane(_owner.aimDir, Vector3.up);
        colliderRb.transform.rotation = Quaternion.LookRotation(colliderDir);
        colliderRb.velocity = colliderDir * _owner.group.arrowSpeed;
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
        Destroy(colliderRb.gameObject, lifetime);
    }
}
