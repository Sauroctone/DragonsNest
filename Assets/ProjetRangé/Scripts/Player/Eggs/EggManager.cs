using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EggManager : MonoBehaviour {


	public float eggTimefactor = 1;
	private float timeEgg;

	public Nest[] levelNests;


	// Use this for initialization
	private void Start () 
	{

	}
	
	// Update is called once per frame
	private void Update () {
		UpdateEggByTime();
	}

	private void UpdateEggByTime ()
	{
		timeEgg += Time.deltaTime;
		
		if(timeEgg<eggTimefactor)
		{
			return;
		}

		RandomLayEgg();

	}

	private Nest RandomAvailaibleNest()
	{
		var _tempList = new List<Nest>();
		foreach (var nest in levelNests)
		{
			if(!nest.egg.gameObject.activeInHierarchy)
			{
				_tempList.Add(nest);
			}	
		}
		
		if (_tempList.Count == 0)
		{
			return null;
		}
		
		int randInt = Random.Range(0,_tempList.Count);

		return _tempList[randInt];
	}

	private void RandomLayEgg()
	{
		timeEgg = 0.0f;

		var actualNest = RandomAvailaibleNest();
		if(actualNest == null) {return;}

		actualNest.Action();
	}
}
