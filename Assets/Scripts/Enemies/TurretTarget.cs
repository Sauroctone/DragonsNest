using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTarget : MonoBehaviour {

	public bool isOnTurretRange;
	private Turret turret;

	void OnTriggerEnter (Collider col)
	{
		if(col.GetComponent<Turret>()== null) return;
		isOnTurretRange = true;
		turret = col.GetComponent<Turret>();
		turret.AddToTargetList(this.transform);
	}

	
	void OnTriggerExit (Collider col)
	{
		if(col.GetComponent<Turret>()== null) return;
		CallRemoveAndChangeTarget();
	}

	public void CallRemoveAndChangeTarget ()
	{
		if(isOnTurretRange)
		{
			isOnTurretRange = false;
			turret.RemoveAndChangeTarget(this.transform);
		}
	}
}
