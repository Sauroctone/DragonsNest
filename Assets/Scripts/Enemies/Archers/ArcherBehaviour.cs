using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArcherBehaviour : LivingBeing {

    public bool isBannerman;

    [Header("Movement")]

    public float speed;
    public float maxWanderRadius;
    public float aimRotLerp;
    public float moveRotLerp;

    [Header("Flight")]
    public float flightSpeed;

    [HideInInspector]
    public Vector3 aimDir;

    Vector3 destination;
    Vector2 randomPoint;
    Vector3 originPos;
    Vector3 viewPos;
    public ArcherGroupBehaviour group;
    public ArcherGroupState groupState;
    public Transform debugIntercept;
    public Transform currentTarget;
    Renderer rend;
    public NavMeshAgent navAgent;

    public override void Start()
    {
        originPos = transform.localPosition;
        destination = transform.localPosition;
        group = GetComponentInParent<ArcherGroupBehaviour>();
        group.EventOnStateChanged += OnGroupStateChanged;
        group.archers.Add(this);
        scoringObject = GetComponent<ScoringObject>();

        //Placeholder feedback
        rend = GetComponent<Renderer>();
    }

    public override void Update ()
    {
        base.Update();

        switch (groupState)
        {
            case ArcherGroupState.MOVING:
                MoveRandomly();
                if (transform.localEulerAngles != Vector3.zero && group.nav.velocity != Vector3.zero)
                    transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.LookRotation(group.nav.velocity), moveRotLerp);
                break;

            case ArcherGroupState.FLEEING_INDIVIDUALLY:
                DieOutOfView();
                break;
        }
    }

    void MoveRandomly()
    {
        if (Vector3.Distance(transform.localPosition, destination) < speed * Time.deltaTime)
        {
            randomPoint = Random.insideUnitCircle * maxWanderRadius;
            destination = originPos + new Vector3(randomPoint.x, 0f, randomPoint.y);
        }

        transform.localPosition += (destination - transform.localPosition).normalized * speed * Time.deltaTime;
    }

    void OnGroupStateChanged(ArcherGroupState _state)
    {
        groupState = _state;
        switch (_state)
        {
            case ArcherGroupState.SHOOTING:
                if(!isBannerman)
                    StartCoroutine(IAimAndShoot());
                break;

            case ArcherGroupState.FLEEING_INDIVIDUALLY:
                LeaveGroup(); 
                break;
        }
    }

    void LeaveGroup()
    {
        if (navAgent != null)
        {
            navAgent.enabled = true;
            navAgent.speed = flightSpeed;
        }
        transform.parent = null;

        float rangeX = Random.Range(-1, 1);
        float rangeZ = Random.Range(-1, 1);
        rangeX = Mathf.Clamp(rangeX, 0.5f * Mathf.Sign(rangeX), 1f * Mathf.Sign(rangeX));
        rangeZ = Mathf.Clamp(rangeZ, 0.5f * Mathf.Sign(rangeZ), 1f * Mathf.Sign(rangeZ));

        Vector3 flightPos = GameManager.Instance.player.transform.position + new Vector3 (rangeX, 0f, rangeZ) * 50f;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(flightPos, out hit, 100f, NavMesh.AllAreas))
        {
            if (navAgent != null)
                navAgent.SetDestination(hit.position);
            //debugSphere.position = targetPosOnNav;
        }
    }

    void DieOutOfView()
    {
        viewPos = GameManager.Instance.mainCamera.WorldToViewportPoint(transform.position);
        if (viewPos.x > 1 || viewPos.x < 0 || viewPos.y > 1 || viewPos.y < 0)
            Die();
    }

    public virtual IEnumerator IAimAndShoot()
    {        
        currentTarget = group.currentTarget;
        float time = 0f;
        float rand = Random.Range(group.minShootTime, group.maxShootTime);

        Vector3 targetPosition = Vector3.zero;
        bool lockedTarget = false;
        while (time < group.aimTime + rand && currentTarget != null)
        {
            time += Time.deltaTime;
            if (time >= group.aimTime && !lockedTarget)
            {
                targetPosition = currentTarget.position;
                lockedTarget = true;
            }

            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.LookRotation((Vector3.ProjectOnPlane(currentTarget.position, Vector3.up) - transform.position).normalized), aimRotLerp);
            yield return null;
        }

        if (currentTarget != null && group != null)
        {
            Vector3 interceptPoint = FirstOrderIntercept(group.transform.position, Vector3.zero, group.arrowSpeed, targetPosition, currentTarget == group.player ? group.playerRb.velocity : Vector3.zero);
            aimDir = (interceptPoint - group.transform.position).normalized;
            GameObject proj = Instantiate(group.arrow, transform.position, Quaternion.identity);
            proj.GetComponent<ArrowBehaviour>().Init(group.arrowLifetime, aimDir, group.arrowSpeed, currentTarget, group.visualTrajectory);
            if (debugIntercept != null)
                debugIntercept.position = interceptPoint;
        }
    }

    private void OnDisable()
    {
        if (group != null)
            group.EventOnStateChanged -= OnGroupStateChanged;
    }

    public override void Die()
    {
        if(isBannerman)
        {
            scoringObject.scoreAmount = 5*group.archers.Count;
            GameManager.Instance.scoreManager.SetCombo();
        }
        else
        {
            scoringObject.scoreAmount = 1;
        }
        base.Die();
        if (group != null)
            group.archers.Remove(this);

        Destroy(gameObject);
    }

    public override void UpdateHealthUI(int _damage)
    {
        // NO HEALTHBAR (yet)
    }

    //first-order intercept using absolute target position
    public Vector3 FirstOrderIntercept (Vector3 shooterPosition, Vector3 shooterVelocity, float shotSpeed, Vector3 targetPosition, Vector3 targetVelocity)
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
    public float FirstOrderInterceptTime (float shotSpeed, Vector3 targetRelativePosition, Vector3 targetRelativeVelocity)
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