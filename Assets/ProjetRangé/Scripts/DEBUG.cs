using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{

		if(Input.GetKeyDown(KeyCode.U))
		{
			GameManager.Instance.player.life = 0;
		}
		
		if(Input.GetKeyDown(KeyCode.I))
		{
			GameManager.Instance.scoreManager.score += 1000;			
		}

	}
}
