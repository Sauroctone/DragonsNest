using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EggManager : MonoBehaviour {


	public float eggTimefactor = 1;
	public PlayerController playerController;


	[Header("Refereces")]
	public Slider eggSlider;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		UpdateEggByTime();
	}

	void UpdateEggByTime ()
	{
		if(eggSlider.value<1 && playerController.playerState == PlayerStates.FLYING)
		{
			eggSlider.value += Time.deltaTime/eggTimefactor;
		}
	}
}
