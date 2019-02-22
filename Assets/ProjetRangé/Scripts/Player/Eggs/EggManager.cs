using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EggManager : MonoBehaviour {

	[Header("WaveStuff")]
	public AnimationCurve timeFactorByWave;
	public int maxWaveForEggTimeChange;
	public float minTimeBetEgg = 1;
	public float maxTimeBetEgg = 1;

	public bool waitDuringInterWave;
	
	private float currentTimeBetEgg;
    public delegate void OnNewEgg();
    public event OnNewEgg EventOnNewEgg;


	[Header("LevelStuff")]
	public Nest[] levelNests;


	// Use this for initialization
	private void Start () 
	{
		maxTimeBetEgg *= GameManager.Instance.paraMan.eggSpeed;
		minTimeBetEgg *= GameManager.Instance.paraMan.eggSpeed;
		
		maxTimeBetEgg = maxTimeBetEgg-minTimeBetEgg;
        
		GameManager.Instance.eggMan = this;
		
		if(maxTimeBetEgg<=0)
		{
			maxTimeBetEgg =0;
		}

	}
	public void LaunchEgg()
	{
		StartCoroutine(UpdateEggByTime());
	}

	private bool CheckIfInterWave()
	{
		if(GameManager.Instance.spawnMan.waveState == WaveState.RESTING)
		{
			return false;
		}
		else
		{
			return true;
		}
	}
	private void SetCurrentTime ()
	{
		if (GameManager.Instance.spawnMan.currentWave > maxWaveForEggTimeChange)
		{
			currentTimeBetEgg = maxTimeBetEgg;
			return;
		}
		else
		{
			currentTimeBetEgg = minTimeBetEgg+maxTimeBetEgg*timeFactorByWave.Evaluate(GameManager.Instance.spawnMan.currentWave/maxWaveForEggTimeChange);
		}
	}
	
	private IEnumerator UpdateEggByTime ()
	{
		SetCurrentTime();
		if(waitDuringInterWave)
		{
			Debug.Log("Lay Egg Wait");
			yield return new WaitWhile(CheckIfInterWave);
		}
		yield return new WaitForSeconds(currentTimeBetEgg);
		RandomLayEgg();
		
		StartCoroutine(UpdateEggByTime());
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

		var actualNest = RandomAvailaibleNest();
		if(actualNest == null) {return;}

		GameManager.Instance.spawnMan.eggs.Add(actualNest.Action());

        if (EventOnNewEgg != null)
            EventOnNewEgg();
	}
	
	public void RandomLayEggTuto()
	{
		var actualNest = RandomAvailaibleNest();
		if(actualNest == null) {return;}

		actualNest.ActionTuto();
	}

}
