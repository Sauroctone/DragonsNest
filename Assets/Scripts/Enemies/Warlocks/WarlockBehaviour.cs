using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlockBehaviour : ArcherBehaviour {

    public override IEnumerator IAimAndShoot()
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
            Vector3 interceptPoint = FirstOrderIntercept(transform.position, Vector3.zero, group.arrowSpeed, targetPosition, currentTarget == group.player ? group.playerRb.velocity : Vector3.zero);
            aimDir = (interceptPoint - transform.position).normalized;
            GameObject proj = Instantiate(group.arrow, transform.position, Quaternion.identity);
            proj.GetComponent<ArrowBehaviour>().Init(group.arrowLifetime, aimDir, group.arrowSpeed, currentTarget, group.visualTrajectory);
            if (debugIntercept != null)
                debugIntercept.position = interceptPoint;
        }
    }

    public override void Die()
    {
        if (isBannerman)
        {
            scoringObject.scoreAmount = 5 * group.archers.Count;
            GameManager.Instance.scoreManager.SetCombo();
            GameManager.Instance.scoreManager.warlockDeathCount++;
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
}
