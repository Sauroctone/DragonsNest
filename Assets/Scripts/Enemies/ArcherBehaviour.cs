﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherBehaviour : LivingBeing {

    [Header("Movement")]

    public float speed;
    public float maxWanderRadius;
    public float aimRotLerp;
    public float moveRotLerp;

    [HideInInspector]
    public Vector3 aimDir;

    Vector3 destination;
    Vector2 randomPoint;
    Vector3 originPos;
    public ArcherGroupBehaviour group;
    ArcherGroupState groupState;
    public Transform debugIntercept;
    public Transform currentTarget;
    Renderer rend;

    public override void Start()
    {
        originPos = transform.localPosition;
        destination = transform.localPosition;
        group = GetComponentInParent<ArcherGroupBehaviour>();
        group.EventOnStateChanged += OnGroupStateChanged;
        group.archers.Add(this);

        //Placeholder feedback
        rend = GetComponent<Renderer>();
    }

    void Update ()
    {
       if (groupState == ArcherGroupState.Moving)
       {
            MoveRandomly();
            if (transform.localEulerAngles != Vector3.zero)
                transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.LookRotation(group.nav.velocity), moveRotLerp);
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
        switch (_state)
        {
            case ArcherGroupState.Moving:
                groupState = ArcherGroupState.Moving;
                break;

            case ArcherGroupState.Shooting:
                groupState = ArcherGroupState.Shooting;
                StartCoroutine(IAimAndShoot());
                break;
        }
    }

    IEnumerator IAimAndShoot()
    {
        //Placeholder feedback
        rend.material = group.aimMat;
        
        currentTarget = group.currentTarget;
        float time = 0f;
        float rand = Random.Range(group.minShootTime, group.maxShootTime);

        while (time < group.aimTime + rand && currentTarget != null)
        {
            time += Time.deltaTime;
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.LookRotation((Vector3.ProjectOnPlane(currentTarget.position, Vector3.up) - transform.position).normalized), aimRotLerp);
            yield return null;
        }

        if (currentTarget != null)
        {
            Vector3 interceptPoint = FirstOrderIntercept(group.transform.position, Vector3.zero, group.arrowSpeed, currentTarget.position, currentTarget == group.player ? group.playerRb.velocity : Vector3.zero);
            aimDir = (interceptPoint - group.transform.position).normalized;
            GameObject proj = Instantiate(group.arrow, transform.position, Quaternion.identity);
            proj.GetComponent<ArrowBehaviour>().Init(this);
            if (debugIntercept != null)
                debugIntercept.position = interceptPoint;
        }
        
        //Placeholder feedback
        rend.material = group.normalMat;
    }

    private void OnDisable()
    {
        group.EventOnStateChanged -= OnGroupStateChanged;
    }

    public override void Die()
    {
        group.archers.Remove(this);
        Destroy(gameObject);
    }

    public override void UpdateHealthUI(int _damage)
    {
        // NO HEALTHBAR (yet)
    }

    //first-order intercept using absolute target position
    public static Vector3 FirstOrderIntercept (Vector3 shooterPosition, Vector3 shooterVelocity, float shotSpeed, Vector3 targetPosition, Vector3 targetVelocity)
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
    public static float FirstOrderInterceptTime (float shotSpeed, Vector3 targetRelativePosition, Vector3 targetRelativeVelocity)
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