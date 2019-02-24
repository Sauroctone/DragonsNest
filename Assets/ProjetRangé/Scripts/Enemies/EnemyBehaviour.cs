using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

[System.Serializable]
public class EnemyTarget
{
    public EnemyTargetType type;
    public List<Transform> instancesInRange = new List<Transform>();
}

public class EnemyBehaviour : MonoBehaviour {

    //  [Header("Player")]
    //    public float distanceToAggroPlayer;

    bool subscribedForEggs;

    public float moveSpeed;

    [Header("Target Priority")]
    public Transform currentTarget;
    public EnemyTarget[] autoTargets;
    public EnemyTarget[] overrideTargets;

    [Header("References")]
    [HideInInspector]
    public SpawnManager spawnMan;
    [HideInInspector]
    public Transform player;
    [HideInInspector]
    public Rigidbody playerRb;

    public virtual void Init()
    {
        GetNewTarget();
        moveSpeed = moveSpeed*GameManager.Instance.paraMan.enemySpeed;
        foreach (EnemyTarget target in autoTargets)
        {
            if(target.type == EnemyTargetType.EGG)
            {
                GameManager.Instance.eggMan.EventOnNewEgg += Event_OnNewEgg;
                break;
            }
        }
    }

    public virtual void Update()
    {
        if (currentTarget != null && !currentTarget.gameObject.activeSelf)
            currentTarget = null;

        //Get new target when it's dead
        if (currentTarget == null && !WasTargetOverridden())
        {
            GetNewTarget();
        }

        //Debug.Log(currentTarget);
    }

    public virtual void OnTriggerEnter(Collider col)
    {
        Transform colTarget = null;
        foreach (EnemyTarget target in overrideTargets)
        {
            switch (target.type)
            {
                case EnemyTargetType.ANCIENT:
                    if (col.tag == "Ancient")
                    {
                        colTarget = col.transform;
                        target.instancesInRange.Add(colTarget);
                    }
                    break;

                case EnemyTargetType.EGG:
                    if (col.tag == "Egg")
                    {
                        colTarget = col.transform;
                        target.instancesInRange.Add(colTarget);
                    }
                    break;

                case EnemyTargetType.PLAYER:
                    if (col.tag == "Dragon")
                    {
                        colTarget = col.transform;
                        player = colTarget;
                        target.instancesInRange.Add(col.transform);
                    }
                    break;
            }
            if (colTarget != null)
                break;
        }
        WasTargetOverridden();
    }

    public virtual void OnTriggerExit(Collider col)
    {
        if (currentTarget == col.transform)
        {
            currentTarget = null;
        }
        foreach (EnemyTarget target in overrideTargets)
        {
            if (target.instancesInRange.Contains(col.transform))
            {
                target.instancesInRange.Remove(col.transform);
                break;
            }
        }
        WasTargetOverridden();
    }

    public void GetNewTarget()
    {
        foreach (EnemyTarget target in autoTargets)
        {
            currentTarget = GetAutoTarget(target);
            if (currentTarget != null)
                break;
        }
    }

    public Transform GetAutoTarget(EnemyTarget autoTarget)
    {
        if (autoTarget == null)
            return null;
        Transform target = null;

        switch (autoTarget.type)
        {            
            case EnemyTargetType.PLAYER:
                return player;

            case EnemyTargetType.EGG:
                if (spawnMan.eggs.Count > 0)
                    target = spawnMan.eggs[0];
                else
                    return null;

                if (spawnMan.eggs.Count > 1)
                {
                    foreach (Transform targ in spawnMan.eggs)
                    {
                        if (Vector3.Distance(targ.position, transform.position) < Vector3.Distance(target.position, transform.position))
                            target = targ;
                    }
                }
                return target;

            case EnemyTargetType.ANCIENT:

                if (spawnMan.ancients.Count > 0)
                    target = spawnMan.ancients[0];
                else
                    return null;

                foreach (Transform ancient in spawnMan.ancients)
                {
                    if (Vector3.Distance(ancient.position, transform.position) < Vector3.Distance(target.position, transform.position))
                        target = ancient.transform;
                }
                return target;
        }
        return null;
    }

    public bool WasTargetOverridden()
    {
        foreach(EnemyTarget target in overrideTargets)
        {
            for (int i = target.instancesInRange.Count-1; i >= 0; i--)
            {
                if (target.instancesInRange[i] == null)
                    target.instancesInRange.RemoveAt(i);
            }
            if (target.instancesInRange.Count > 0)
            {
                currentTarget = target.instancesInRange[0];
                return true;
            }
        }
        return false;
    }
    
    public void Event_OnNewEgg()
    {
        currentTarget = null;
        GetNewTarget();
    }

    public void Die()
    {
        spawnMan.RemoveEnemyFromList(this);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
            GameManager.Instance.eggMan.EventOnNewEgg -= Event_OnNewEgg;    
    }
}