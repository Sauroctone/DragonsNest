﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToAncient : MonoBehaviour {

	public PlayerController playerController;
	public GameObject ancient;

	// Use this for initialization
	void Start () {
		
	}
	
	private bool CheckIfAncient ()
	{
		if (playerController.babyDragonMan.babyDragons.Count > 0)
        //if (playerController.canLand)
		if(Input.GetButtonDown("Fire2")) return true;
		return false;
	}
}
