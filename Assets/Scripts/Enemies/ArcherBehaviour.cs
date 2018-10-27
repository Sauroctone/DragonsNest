using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherBehaviour : MonoBehaviour {

    public float speed;
    public float maxWanderRadius;
    public float aimRotLerp;

    Vector3 destination;
    Vector2 randomPoint;
    Vector3 originPos;
    ArcherGroupBehaviour group;
    ArcherGroupState groupState;


    void Start()
    {
        originPos = transform.localPosition;
        group = GetComponentInParent<ArcherGroupBehaviour>();
        group.EventOnStateChanged += OnGroupStateChanged;
    }

    void Update ()
    {
       if (groupState == ArcherGroupState.Moving)
       {
            MoveRandomly();
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
        float time = 0f;
        while (time < group.aimTime)
        {
            time += Time.deltaTime;
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.LookRotation((Vector3.ProjectOnPlane(group.target.position, Vector3.up) - transform.position).normalized), aimRotLerp);
            yield return null;
        }
        Vector3 aimDir = ((group.target.position + group.targetRb.velocity * group.aimLead) - group.transform.position).normalized;
        yield return new WaitForSeconds(Random.Range(group.minShootTime, group.maxShootTime));
        GameObject proj = Instantiate(group.arrow, transform.position, Quaternion.identity);
        proj.transform.rotation = Quaternion.LookRotation(aimDir);
        proj.GetComponent<Rigidbody>().velocity = aimDir * group.arrowSpeed;
    }

    private void OnDisable()
    {
        group.EventOnStateChanged -= OnGroupStateChanged;
    }
}