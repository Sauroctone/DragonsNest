using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EggManager : MonoBehaviour {


	public float eggTimefactor = 1;


	[Header("Refereces")]
	public PlayerController playerController;
	public Image eggSlider;
	public Color eggColor;
	[System.NonSerialized]
	public Color startEggColor;
	public GameObject eggGlow;

	// Use this for initialization
	void Start () {
		startEggColor = eggSlider.color;
	}
	
	// Update is called once per frame
	void Update () {
		UpdateEggByTime();
		//Debug.Log(this.name);
	}

	void UpdateEggByTime ()
	{
		if(eggSlider.fillAmount<1 && playerController.playerState == PlayerStates.FLYING)
		{
			if(eggGlow.activeInHierarchy) eggGlow.SetActive(false);
			eggSlider.fillAmount += Time.deltaTime/eggTimefactor;
		}
		if(eggSlider.fillAmount>=1)
		{
			eggGlow.SetActive(true);
			eggSlider.color = eggColor;
		}
	}
}
