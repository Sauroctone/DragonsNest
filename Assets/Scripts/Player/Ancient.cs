using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   

public class Ancient : LivingBeing {

    public float minHealthbarScale;

    [Header("Shooting")]
    public List<EnemyBehaviour> targets;
    bool isShooting;
    public float timeBetweenCols;
    float fireTime;
    public float fireColSpeed;

    [Header("References")]
    public GameObject projectile;
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
                GameObject fireCol = Instantiate(projectile, fireOrigin.position, Quaternion.identity);
                fireCol.GetComponent<Rigidbody>().velocity = (target.position - fireOrigin.position).normalized * fireColSpeed;
                fireTime = 0;
            }
            else
                fireTime += Time.deltaTime;

            transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane((target.position - transform.position).normalized, Vector3.up));
        }
    }

    public void Shoot()
    {
        isShooting = true;
        fireTime = 0;
    }

    public void StopShooting()
    {
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
        if (targ != null)
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
    
    //Overrides

    public override void UpdateHealthUI(int _damage)
    {
        lifeBar.rectTransform.localScale = Vector3.Lerp(new Vector3(minHealthbarScale, minHealthbarScale, minHealthbarScale), Vector3.one, life / maxLife);
    }

    public override void Die()
    {
        base.Die();
        Destroy(gameObject);
    }
}