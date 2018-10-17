using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherBehaviour : MonoBehaviour {

    public float speed;
    public float maxWanderRadius;
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
        yield return null;
    }

    private void OnDisable()
    {
        group.EventOnStateChanged -= OnGroupStateChanged;
    }
}