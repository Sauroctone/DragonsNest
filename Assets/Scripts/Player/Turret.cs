using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    public float maxWanderRadius;
    Vector3 destination;
    Vector2 randomPoint;
    bool isShooting;

    [Header("Shooting")]
    public float timeBetweenCols;
    float fireTime;
    public float fireColSpeed;
    Coroutine stopShootingCor;
    Coroutine startShootingCor;
    public float minShootDelay;
    public float maxShootDelay;
    Transform shootTarget;
    Rigidbody playerRb;

    [Header("References")]
    public GameObject fireCollider;
    public ParticleSystem firePartSys;
    public Transform fireOrigin;
    public Transform target;

    private void Start()
    {
        randomPoint = Random.insideUnitCircle * maxWanderRadius;
        destination = new Vector3(randomPoint.x, 0f, randomPoint.y);
    }

    private void Update()
    {
        //CheckShoot();
        if(Input.GetKeyDown(KeyCode.Y))
        {
		    Shoot (target);
        }
        if(Input.GetKeyUp(KeyCode.Y))
        {
            StopShooting();
        }
    }

    void CheckShoot()
    {
        if (isShooting)
        {
            if (fireTime >= timeBetweenCols)
            {
                GameObject fireCol = Instantiate(fireCollider, fireOrigin.position, Quaternion.identity);
                fireCol.GetComponent<Rigidbody>().velocity = playerRb.velocity + (shootTarget.position - fireOrigin.position).normalized * fireColSpeed;
                fireTime = 0;
            }
            else
                fireTime += Time.deltaTime;

            transform.LookAt(shootTarget);
        }
        else
        {
            transform.LookAt(transform.position + transform.forward);
        }
    }

    public void Shoot(Transform _target)
    {
        if (shootTarget == null)        //Transfer to spawn function
            shootTarget = _target;

        if (stopShootingCor != null)
            StopCoroutine(stopShootingCor);
        if (startShootingCor != null)
            StopCoroutine(startShootingCor);
        startShootingCor = StartCoroutine(IStartShooting());
    }

    public void StopShooting()
    {
        stopShootingCor = StartCoroutine(IStopShooting());
    }

    IEnumerator IStartShooting()
    {
        yield return new WaitForSeconds(Random.Range(minShootDelay, maxShootDelay));
        firePartSys.Play();
        isShooting = true;
        fireTime = 0;
    }

    IEnumerator IStopShooting()
    {
        yield return new WaitForSeconds(Random.Range(minShootDelay, maxShootDelay));
        firePartSys.Stop();
        isShooting = false;
    }
}
