using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {

	public int timer;
	private bool paused = false;
	// Use this for initialization
	public void LaunchTimer () 
	{
		timer = 0;
		StartCoroutine(SetTimer());

	}

	private IEnumerator SetTimer()
	{
		yield return new WaitUntil(()=> !paused);
		
		yield return new WaitForSeconds(1);
		timer +=1;

		StartCoroutine(SetTimer());
		
		yield break;
	}
	
}
