using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

    [Header("Player")]
    public float distanceToAggroPlayer;

    [Header("References")]
    public Transform currentTarget;
    Transform prevTarget;
    [HideInInspector]
    public SpawnManager spawnMan;
    [HideInInspector]
    public Transform player;
    [HideInInspector]
    public Rigidbody playerRb;

    public virtual void Init()
    {
        AskForNewTarget();
    }

    public virtual void AskForNewTarget()
    {
        currentTarget = spawnMan.GetNewTarget(transform.position);
    }

    public virtual void Update()
    {
        //Get new target when it's dead
        if (currentTarget == null)
            AskForNewTarget();

        //Aggro player on proximity
        if (Vector3.Distance(transform.position, player.position) < distanceToAggroPlayer)
        {
            if (currentTarget != player)
            {
                prevTarget = currentTarget;
                currentTarget = player;
            }
        }
        else
        {
            if (currentTarget == player)
            {
                currentTarget = prevTarget;
            }
        }
    }

    public void Die()
    {
        spawnMan.enemies.Remove(this);
        Destroy(gameObject);
    }
}
