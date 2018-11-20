using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : LivingBeing {

    public float maxWanderRadius;
    public List<Transform> targets;
    Vector3 destination;
    Vector2 randomPoint;
    bool isShooting;
    bool canShoot;

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
    Transform target;

    public override void Start()
    {
        base.Start();
        targets = new List<Transform>();
        randomPoint = Random.insideUnitCircle * maxWanderRadius;
        destination = new Vector3(randomPoint.x, 0f, randomPoint.y);
    }

    private void FixedUpdate()
    {
        CheckShoot();
        if (Input.GetButtonDown("Fire1"))
        {
            if(canShoot) Shoot (target);
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopShooting();
        }
    }

    void CheckShoot()
    {
        if (isShooting )
        {
            if (fireTime >= timeBetweenCols)
            {
                GameObject fireCol = Instantiate(fireCollider, fireOrigin.position, Quaternion.identity);
                fireCol.GetComponent<Rigidbody>().velocity = (shootTarget.position - fireOrigin.position).normalized * fireColSpeed;
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

    public void AddToTargetList (Transform targ)
    {
        if(targets.Count<=0)
        {
            targets.Add(targ);
            target = targets[0];
            canShoot = true;
        }else targets.Add(targ);
    }

    public void RemoveAndChangeTarget(Transform targ)
    {
        targets.Remove(targ);
        if(targets.Count > 0)
        {
        target = targets[0];
        } else canShoot = false;

    }

    public void StopShooting()
    {
        stopShootingCor = StartCoroutine(IStopShooting());
    }

    IEnumerator IStartShooting()
    {
        yield return new WaitForSeconds(Random.Range(minShootDelay, maxShootDelay));
        Debug.Log("shoot");
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