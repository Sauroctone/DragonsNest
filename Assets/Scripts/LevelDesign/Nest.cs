using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nest : MonoBehaviour {

	public GameObject eggs;

	public PlayerController playerController;
	public void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "Dragon" && !playerController.canLand)
		{
			playerController.canLand = true;
			playerController.nest = this;
		}
	}

	public void OnTriggerExit(Collider col)
	{
		if(col.gameObject.tag == "Dragon" && playerController.canLand)
		{
			playerController.canLand = false;
			//playerController.nest = null;
		}
	}

	public void Action()
	{
		if(eggs.activeInHierarchy==false)
		{
			eggs.GetComponent<Eggs>().Start();
			eggs.SetActive(true);
		}
		else 
		if (eggs.GetComponent<Eggs>().canBeADrone)
		{
			eggs.GetComponent<Eggs>().TransformDrone(playerController);
		}
	}
}
