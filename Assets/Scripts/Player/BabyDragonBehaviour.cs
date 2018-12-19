using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyDragonBehaviour : MonoBehaviour {

    public float flySpeed;
    public float maxWanderRadius;
    public float rotLerp = 0.1f;
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
    public Vector3 targetOffset;
    Rigidbody playerRb;

    [Header("References")]
    public GameObject fireCollider;
    public ParticleSystem firePartSys;
    public Transform fireOrigin;

    private void Start()
    {
        randomPoint = Random.insideUnitCircle * maxWanderRadius;
        destination = new Vector3(randomPoint.x, 0f, randomPoint.y);
    }

    private void Update()
    {
        CheckShoot();

        if (Vector3.Distance(transform.localPosition, destination) < flySpeed * Time.deltaTime)
        {
            randomPoint = Random.insideUnitCircle * maxWanderRadius;
            destination = new Vector3(randomPoint.x, 0f, randomPoint.y);
        }

        transform.localPosition += (destination - transform.localPosition).normalized * flySpeed * Time.deltaTime;
    }

    void CheckShoot()
    {
        if (isShooting)
        {
            if (fireTime >= timeBetweenCols)
            {
                GameObject fireCol = Instantiate(fireCollider, fireOrigin.position, Quaternion.identity);
                fireCol.GetComponent<Rigidbody>().velocity = playerRb.velocity + ((shootTarget.position + targetOffset) - fireOrigin.position).normalized * fireColSpeed;
                fireTime = 0;
            }
            else
                fireTime += Time.deltaTime;

            transform.LookAt(shootTarget.position + targetOffset);
        }
        else
        {
            if (transform.localEulerAngles != Vector3.zero)
                transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, 0, 0), rotLerp);
        }
    }

    public void Shoot(Transform _target, Rigidbody _rb)
    {
        if (shootTarget == null)        //Transfer to spawn function
            shootTarget = _target;

        if (playerRb == null)           //Same
            playerRb = _rb;

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
