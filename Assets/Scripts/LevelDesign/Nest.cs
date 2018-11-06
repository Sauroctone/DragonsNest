using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nest : MonoBehaviour {

	public PlayerController playerController;
	public void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "Dragon" && !playerController.canLand)
		{
			playerController.canLand = true;
			Debug.Log("CanLand");
		}
	}
}
