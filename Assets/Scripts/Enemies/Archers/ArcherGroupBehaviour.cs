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
    public GameObject bannerMan;

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
    public bool canShoot = true;
    public float shootCooldown;
    public GameObject arrow;
    public float arrowSpeed;
    public float arrowLifetime;
    public float arrowMaxHeight;
    public AnimationCurve visualTrajectory;

    [Header("References")]
    public BannerVisualizer banner;
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
            case ArcherGroupState.MOVING:
                CheckDistanceToTarget();
                break;
            case ArcherGroupState.SHOOTING:
                if (canShoot)
                    CheckDistanceToTarget();
                break;
        }

        if (bannerMan == null || archers.Count == 1)
        {
            state = ArcherGroupState.FLEEING_INDIVIDUALLY;
            if (EventOnStateChanged != null)
                EventOnStateChanged(state);

            Die();
        }
    }

    void CheckDistanceToTarget()
    {
        if (currentTarget != null && Vector3.Distance(currentTarget.position, transform.position) < aimRange 
            && canShoot
            && !Physics.Raycast(transform.position, (currentTarget.position - transform.position).normalized, aimRange, aimObstacleLayer))
        {
            state = ArcherGroupState.SHOOTING;
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
        state = ArcherGroupState.MOVING;
        if (EventOnStateChanged != null)
            EventOnStateChanged(state);
        moveCor = StartCoroutine(IUpdateTargetPosition());
    }

    IEnumerator IUpdateTargetPosition()
    {
        if (currentTarget != null && NavMesh.SamplePosition(currentTarget.position, out hit, 100f, NavMesh.AllAreas))
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
        banner.ChangeBanner(aimMat);
        canShoot = false;
        yield return new WaitForSeconds(aimTime + maxShootTime);
        banner.ChangeBanner(normalMat);
        yield return new WaitForSeconds(postShootIdleTime);
        if (currentTarget == null || Vector3.Distance(currentTarget.position, transform.position) > aimRange)
            StartMoving();
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }
}