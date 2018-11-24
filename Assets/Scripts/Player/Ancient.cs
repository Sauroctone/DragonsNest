using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ancient : LivingBeing {

    public float maxWanderRadius;
    public List<EnemyBehaviour> targets;
    Vector3 destination;
    Vector2 randomPoint;
    bool isShooting;

    [Header("Shooting")]
    public float timeBetweenCols;
    float fireTime;
    public float fireColSpeed;
    public float minShootDelay;
    public float maxShootDelay;
    Rigidbody playerRb;

    [Header("References")]
    public GameObject fireCollider;
    public ParticleSystem firePartSys;
    public Transform fireOrigin;
    Transform target;

    public override void Start()
    {
        base.Start();
        targets = new List<EnemyBehaviour>();
    }

    private void FixedUpdate()
    {
        // if (Input.GetButtonDown("Shoot"))
        if (isShooting && target == null)
            RemoveAndChangeTarget();

        CheckShoot();
    }

    public override void Update()
    {
        base.Update();
    }

    //Custom

    void CheckShoot()
    {
        if (isShooting)
        {
            if (fireTime >= timeBetweenCols)
            {
                GameObject fireCol = Instantiate(fireCollider, fireOrigin.position, Quaternion.identity);
                fireCol.GetComponent<Rigidbody>().velocity = (target.position - fireOrigin.position).normalized * fireColSpeed;
                fireTime = 0;
            }
            else
                fireTime += Time.deltaTime;

            transform.LookAt(target);
        }
    }

    public void Shoot()
    {
        firePartSys.Play();
        isShooting = true;
        fireTime = 0;
    }

    public void StopShooting()
    {
        firePartSys.Stop();
        isShooting = false;
    }

    public void AddToTargetList (EnemyBehaviour targ)
    {
        targets.Add(targ);
        target = targets[0].transform;
        if (!isShooting)
            Shoot();

        targ.AggroAncient(transform);
    }

    public void RemoveAndChangeTarget(EnemyBehaviour targ)
    {
        targets.Remove(targ);
        targ.ForgetAncient(transform);

        if (targets.Count > 0)
        {
            target = targets[0].transform;
            if (!isShooting)
                Shoot();
        }
        else
            StopShooting();
    }

    public void RemoveAndChangeTarget()
    {
        targets.RemoveAt(0);

        if (targets.Count > 0)
        {
            target = targets[0].transform;
            if (!isShooting)
                Shoot();
        }
        else
            StopShooting();
    }

    void OnTriggerEnter (Collider col)
	{
        foreach(EnemyBehaviour _enemy in targets)
        {
            if (_enemy.transform == col.transform.parent)
                return;
        }

        EnemyBehaviour enemy = col.GetComponentInParent<EnemyBehaviour>();
        if (enemy != null)
            AddToTargetList(enemy);
    }
	
	void OnTriggerExit (Collider col)
	{
		if(col.gameObject.tag != "Archer")
            return;

        RemoveAndChangeTarget(col.GetComponent<EnemyBehaviour>());
	}
    
    //Overrides

    public override void UpdateHealthUI(int _damage)
    {
        
    }

    public override void Die()
    {
        base.Die();
        Destroy(gameObject);
    }
}