using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArcherGroupBehaviour : EnemyBehaviour {

    ArcherGroupState state;
    public delegate void OnStateChanged(ArcherGroupState _state);
    public event OnStateChanged EventOnStateChanged;

    [Header("Archers")]
    public List<ArcherBehaviour> archers = new List<ArcherBehaviour>();

    [Header("Movement")]
    public float updatePathFrequency;
    public Transform debugSphere;
    Vector3 targetPosOnNav;
    NavMeshHit hit;
    Coroutine moveCor;
    public float moveRotLerp;

    [Header("Shooting")]
    public LayerMask aimObstacleLayer;
    public float aimRange;
    public float aimTime;
    public float minShootTime;
    public float maxShootTime;
    public float postShootIdleTime;
    bool canShoot = true;
    public float shootCooldown;
    public GameObject arrow;
    public float arrowSpeed;
    public float arrowLifetime;
    public float arrowMaxHeight;
    public AnimationCurve visualTrajectory;

    [Header("References")]
    public NavMeshAgent nav;
    public Material normalMat;
    public Material aimMat;

    public override void Init()
    {
        base.Init();
        moveCor = StartCoroutine(IUpdateTargetPosition());
    }

    public override void Update()
    {
        base.Update();

        switch(state)
        {
            case ArcherGroupState.Moving:
                CheckDistanceToTarget();
                if (nav.velocity != Vector3.zero)
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(nav.velocity.normalized), moveRotLerp);
                break;
            case ArcherGroupState.Shooting:
                break;
        }

        if (archers.Count == 0)
            Die();
    }

    void CheckDistanceToTarget()
    {
        if (Vector3.Distance(currentTarget.position, transform.position) < aimRange 
            && canShoot
            && !Physics.Raycast(transform.position, (currentTarget.position - transform.position).normalized, aimRange, aimObstacleLayer))
        {
            state = ArcherGroupState.Shooting;
            if (EventOnStateChanged != null)
                EventOnStateChanged(state);
            StopCoroutine(moveCor);
            nav.ResetPath();
            StartCoroutine(IAimAndShoot());

            Debug.DrawLine(transform.position, currentTarget.position, Color.red, 2f);
        }
    }

    void StartMoving()
    {
        state = ArcherGroupState.Moving;
        if (EventOnStateChanged != null)
            EventOnStateChanged(state);
        moveCor = StartCoroutine(IUpdateTargetPosition());
    }

    IEnumerator IUpdateTargetPosition()
    {
        if (NavMesh.SamplePosition(currentTarget.position, out hit, 100f, NavMesh.AllAreas))
        {
            targetPosOnNav = hit.position;
            if (nav != null)
                nav.SetDestination(targetPosOnNav);
            //debugSphere.position = targetPosOnNav;
        }
        yield return new WaitForSeconds(updatePathFrequency);
        moveCor = StartCoroutine(IUpdateTargetPosition());
    }

    IEnumerator IAimAndShoot()
    {
        yield return new WaitForSeconds(aimTime);
        canShoot = false;
        yield return new WaitForSeconds(maxShootTime + postShootIdleTime);
        StartMoving();
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }
}
