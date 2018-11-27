using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

    [Header("Player")]
    public float distanceToAggroPlayer;

    [HideInInspector]
    public Transform currentTarget;
    List<Transform> ancientTargets = new List<Transform>();
    
    [Header("References")]
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

        //If no ancient is close : aggro player on proximity
        if (ancientTargets.Count == 0)
        { 
            if (Vector3.Distance(transform.position, player.position) <= distanceToAggroPlayer)
            {
                if (currentTarget != player)
                    currentTarget = player;
            }
            else
            {
                if (currentTarget == player)
                    currentTarget = null;
            }
        }
    }

    public void AggroAncient(Transform _ancient)
    {
        ancientTargets.Add(_ancient);
        currentTarget = ancientTargets[0];
    }

    public void ForgetAncient(Transform _ancient)
    {
        ancientTargets.Remove(_ancient);

        if (ancientTargets.Count > 0)
            currentTarget = ancientTargets[0];
        else
            currentTarget = null;
    }

    public void Die()
    {
        spawnMan.enemies.Remove(this);
        Destroy(gameObject);
    }
}
