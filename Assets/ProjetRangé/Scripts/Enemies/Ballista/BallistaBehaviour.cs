using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BallistaBehaviour : EnemyBehaviour {

    public BallistaState state;
    //public delegate void OnStateChanged(BallistaState _state);
    //public event OnStateChanged EventOnStateChanged;

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
    public float aimRotLerp;
    public float postShootIdleTime;
    Vector3 aimDir;
    bool canShoot = true;
    bool isShooting;
    public float shootCooldown;
    public GameObject arrow;
    public float arrowSpeed;
    public float arrowLifetime;
    public float arrowMaxHeight;
    public AnimationCurve visualTrajectory;

    [Header("References")]
    public BannerVisualizer banner;
    public NavMeshAgent nav;

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
            case BallistaState.MOVING:
                CheckDistanceToTarget();
                if (nav.velocity != Vector3.zero)
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(nav.velocity.normalized), moveRotLerp);
                break;
            case BallistaState.SHOOTING:
                CheckDistanceToTarget();
                break;
        }

        if (bannerMan == null)
        {
            GameManager.Instance.player.PlayEnemiesDeath();
            Die();
        }
    }

    void CheckDistanceToTarget()
    {
        if (currentTarget != null)
        {
            if (Vector3.Distance(currentTarget.position, transform.position) < aimRange 
                && canShoot
                && !Physics.Raycast(transform.position, (currentTarget.position - transform.position).normalized, aimRange, aimObstacleLayer))
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
        state = BallistaState.SHOOTING;

        isShooting = true;
        canShoot = false;

        if (moveCor != null)
            StopCoroutine(moveCor);
        nav.ResetPath();

        StartCoroutine(IAimAndShoot());
    }

    void StartMoving()
    {
        state = BallistaState.MOVING;
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
        float time = 0;
        while (time < aimTime)
        {
            time += Time.deltaTime;
            banner.UpdateBanner(time/aimTime);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.LookRotation((Vector3.ProjectOnPlane(currentTarget.position, Vector3.up) - transform.position).normalized), aimRotLerp);
            yield return null;
        }
        if (currentTarget != null)
        {
            Vector3 interceptPoint = FirstOrderIntercept(transform.position, Vector3.zero, arrowSpeed, currentTarget.position, currentTarget == player ? playerRb.velocity : Vector3.zero);
            aimDir = (interceptPoint - transform.position).normalized;
            GameObject proj = Instantiate(arrow, transform.position, Quaternion.identity);
            proj.GetComponent<ArrowBehaviour>().Init(arrowLifetime, aimDir, arrowSpeed, currentTarget, visualTrajectory);
        }
        banner.UpdateBanner(0f);
        yield return new WaitForSeconds(postShootIdleTime);
        if (currentTarget == null || Vector3.Distance(currentTarget.position, transform.position) > aimRange)
            StartMoving();
        yield return new WaitForSeconds(shootCooldown - postShootIdleTime);
        isShooting = false;
        canShoot = true;
    }

    //first-order intercept using absolute target position
    public Vector3 FirstOrderIntercept(Vector3 shooterPosition, Vector3 shooterVelocity, float shotSpeed, Vector3 targetPosition, Vector3 targetVelocity)
    {
        Vector3 targetRelativePosition = targetPosition - shooterPosition;
        Vector3 targetRelativeVelocity = targetVelocity - shooterVelocity;
        float t = FirstOrderInterceptTime
        (
            shotSpeed,
            targetRelativePosition,
            targetRelativeVelocity
        );
        return targetPosition + t * (targetRelativeVelocity);
    }

    //first-order intercept using relative target position
    public float FirstOrderInterceptTime(float shotSpeed, Vector3 targetRelativePosition, Vector3 targetRelativeVelocity)
    {
        float velocitySquared = targetRelativeVelocity.sqrMagnitude;
        if (velocitySquared < 0.001f)
            return 0f;

        float a = velocitySquared - shotSpeed * shotSpeed;

        //handle similar velocities
        if (Mathf.Abs(a) < 0.001f)
        {
            float t = -targetRelativePosition.sqrMagnitude /
            (
                2f * Vector3.Dot
                (
                    targetRelativeVelocity,
                    targetRelativePosition
                )
            );
            return Mathf.Max(t, 0f); //don't shoot back in time
        }

        float b = 2f * Vector3.Dot(targetRelativeVelocity, targetRelativePosition);
        float c = targetRelativePosition.sqrMagnitude;
        float determinant = b * b - 4f * a * c;

        if (determinant > 0f)
        { //determinant > 0; two intercept paths (most common)
            float t1 = (-b + Mathf.Sqrt(determinant)) / (2f * a),
                    t2 = (-b - Mathf.Sqrt(determinant)) / (2f * a);
            if (t1 > 0f)
            {
                if (t2 > 0f)
                    return Mathf.Min(t1, t2); //both are positive
                else
                    return t1; //only t1 is positive
            }
            else
                return Mathf.Max(t2, 0f); //don't shoot back in time
        }
        else if (determinant < 0f) //determinant < 0; no intercept path
            return 0f;
        else //determinant = 0; one intercept path, pretty much never happens
            return Mathf.Max(-b / (2f * a), 0f); //don't shoot back in time
    }
}