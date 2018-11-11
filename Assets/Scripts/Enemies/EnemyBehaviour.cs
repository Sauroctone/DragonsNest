using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

    [Header("Player")]
    public float distanceToAggroPlayer;

    [Header("References")]
    public Transform ultimateTarget;
    public Transform currentTarget;
    Transform prevTarget;
    public SpawnManager spawnMan;
    public Transform player;
    public Rigidbody playerRb;

    public virtual void Init()
    {
        AskForNewTarget();
        print(player.name);
    }

    public virtual void AskForNewTarget()
    {
        ultimateTarget = spawnMan.GetRandomTarget();
        currentTarget = ultimateTarget;
        print(currentTarget.name);
    }

    public virtual void Update()
    {
        //Get new target when it's dead
        if (ultimateTarget == null)
            AskForNewTarget();
        else if (currentTarget == null)
        {
            currentTarget = ultimateTarget;
            print(currentTarget.name);
        }

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

            //foreach(Transform egg in spawnMan.eggs)
            //{
            //    if (Vector3.Distance(egg.position, transform.position) > )
            //}

        //AGGRO DES OEUFS A PROXIMITE - A CONFIRMER
        }
    }

    public void Die()
    {
        spawnMan.enemies.Remove(this);
        Destroy(gameObject);
    }
}
