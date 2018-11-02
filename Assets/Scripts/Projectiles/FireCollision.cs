using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCollision : Projectile {

    public GameObject groundOnFire;
    public LayerMask groundMask;
    Rigidbody rb;

    private void Start()
    {
        Destroy(gameObject, 5);
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            RaycastHit hit;
            Debug.DrawLine(transform.position - rb.velocity.normalized * 5f, transform.position + rb.velocity.normalized * 5f, Color.black);
            if (Physics.Raycast(transform.position - rb.velocity.normalized * 5f, rb.velocity.normalized, out hit, 5f, groundMask))
            {
                GameObject fire = Instantiate(groundOnFire, hit.point, Quaternion.Euler(-90, 0, 0));
                fire.transform.localScale = transform.localScale;
            }
            Destroy(gameObject);
        }
    }
}
