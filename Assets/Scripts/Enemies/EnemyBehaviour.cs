using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

    [Header("Player")]
    public float distanceToAggroPlayer;

    [Header("References")]
    public Transform currentTarget;
    Transform prevTarget;
    public SpawnManager spawnMan;
    public Transform player;
    public Rigidbody playerRb;

    public virtual void Init()
    {
        AskForNewTarget();
    }

    public virtual void AskForNewTarget()
    {
        currentTarget = spawnMan.GetNewTarget(transform.position);
        print(currentTarget.name);
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
                print(currentTarget.name);
            }
        }
        else
        {
            if (currentTarget == player)
            {
                currentTarget = prevTarget;
                print(currentTarget.name);
            }
        }
    }

    public void Die()
    {
        spawnMan.enemies.Remove(this);
        Destroy(gameObject);
    }
}
