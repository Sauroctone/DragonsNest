using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArcherGroupBehaviour : EnemyBehaviour {

    public ArcherGroupState state;
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
    bool canShoot = true;
    bool isShooting;
    public float shootCooldown;
    public GameObject arrow;
    public float arrowSpeed;
    public float arrowLifetime;
    public float arrowMaxHeight;
    public AnimationCurve visualTrajectory;
    [HideInInspector]
    public Vector3 targetPosition;
    [HideInInspector]
    public bool lockedTarget;
    [HideInInspector]
    public Vector3 targetVelocity;

    [Header("References")]
    public BannerVisualizer banner;
    public NavMeshAgent nav;
    public Material normalMat;
    public Material aimMat;
    private GameObject Player;

    public override void Init()
    {
        base.Init();
        nav.speed = moveSpeed;
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
                CheckDistanceToTarget();
                break;
        }

        if (bannerMan == null || archers.Count == 1)
        {
            state = ArcherGroupState.FLEEING_INDIVIDUALLY;
            if (EventOnStateChanged != null)
                GameManager.Instance.player.PlayEnemiesDeath();
                EventOnStateChanged(state);
            Die();
        }
    }

    void CheckDistanceToTarget()
    {
        if (currentTarget != null)
        {
            if (Vector3.Distance(currentTarget.position, transform.position) < aimRange 
                && canShoot)
                //&& !Physics.Raycast(transform.position, (currentTarget.position - transform.position).normalized, aimRange, aimObstacleLayer))
            {
                Debug.DrawLine(transform.position, currentTarget.position, Color.red, 2f);
                AimAndShoot();
            }
            else
            {
                if (moveCor == null && !isShooting)
                    StartMoving();
            }
        }
    }

    void AimAndShoot()
    {
        state = ArcherGroupState.SHOOTING;
        if (EventOnStateChanged != null)
            EventOnStateChanged(state);

        isShooting = true;
        canShoot = false;

        if (moveCor != null)
            StopCoroutine(moveCor);
        nav.ResetPath();

        StartCoroutine(IAimAndShoot());
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
            if (nav != null && nav.gameObject.activeSelf)
                nav.SetDestination(targetPosOnNav);
            //debugSphere.position = targetPosOnNav;
        }
        yield return new WaitForSeconds(updatePathFrequency);
        moveCor = StartCoroutine(IUpdateTargetPosition());
    }

    IEnumerator IAimAndShoot()
    {
        lockedTarget = false;
        float time = 0;
        while (time < aimTime)
        {
            time += Time.deltaTime;
            banner.UpdateBanner(time/aimTime);
            yield return null;
        }
        yield return new WaitForSeconds(maxShootTime);
        banner.UpdateBanner(0f);
        yield return new WaitForSeconds(postShootIdleTime);
        if (currentTarget == null || Vector3.Distance(currentTarget.position, transform.position) > aimRange)
            StartMoving();
        yield return new WaitForSeconds(shootCooldown - postShootIdleTime);
        isShooting = false;
        canShoot = true;
    }
}