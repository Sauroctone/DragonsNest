using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArcherGroupBehaviour : MonoBehaviour {

    ArcherGroupState state;
    public delegate void OnStateChanged(ArcherGroupState _state);
    public event OnStateChanged EventOnStateChanged;

    [Header("Movement")]
    public float updatePathFrequency;
    public Transform debugSphere;
    Vector3 targetPosOnNav;
    NavMeshHit hit;
    Coroutine moveCor;
    
    [Header("Shooting")]
    public float aimRange;
    public float aimTime;
    public float minShootTime;
    public float maxShootTime;
    bool canShoot = true;
    public float shootCooldown;

    [Header("References")]
    public Transform target;
    public NavMeshAgent nav;

    void Start()
    {
        moveCor = StartCoroutine(IUpdateTargetPosition());
    }

    void Update()
    {
        switch(state)
        {
            case ArcherGroupState.Moving:
                CheckDistanceToTarget();
                break;
            case ArcherGroupState.Shooting:
                break;
        }
    }

    void CheckDistanceToTarget()
    {
        if (Vector3.Distance(target.position, transform.position) < aimRange)
        {
            state = ArcherGroupState.Shooting;
            EventOnStateChanged(state);
            StopCoroutine(moveCor);
            nav.ResetPath();
            StartCoroutine(IAimAndShoot());
        }
    }

    void StartMoving()
    {
        state = ArcherGroupState.Moving;
        EventOnStateChanged(state);
        moveCor = StartCoroutine(IUpdateTargetPosition());
    }

    IEnumerator IUpdateTargetPosition()
    {
        if (NavMesh.SamplePosition(target.position, out hit, 100f, NavMesh.AllAreas))
        {
            targetPosOnNav = hit.position;
            nav.SetDestination(targetPosOnNav);
            debugSphere.position = targetPosOnNav;
        }
        yield return new WaitForSeconds(updatePathFrequency);
        moveCor = StartCoroutine(IUpdateTargetPosition());
    }

    IEnumerator IAimAndShoot()
    {
        yield return new WaitForSeconds(aimTime);
        canShoot = false;
        yield return new WaitForSeconds(maxShootTime);
        StartMoving();
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }
}
