using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringObject : MonoBehaviour {

	public int scoreAmount = 1;

	public ParticleSystem goldPS;	
	public ParticleSystem tinPS;
	public ParticleSystem cooperPS;
	
	

	internal void SetScore()
	{
		var currentScore = scoreAmount + GameManager.Instance.scoreManager.comboAmount ;
		GameManager.Instance.scoreManager.score += currentScore;
		//Pas opti mais necessaire pour le moment 
		SetParticle(transform.position,currentScore);

	}

	internal void SetParticle(Vector3 position, int score)
	{
		int goldAmount = 0;
		int tinAmount = 0;
		int cooperAmount = 0;
		
		var goldEm = goldPS.emission;
		var tinEm = tinPS.emission;
		var cooperEm = cooperPS.emission;
		
		//setGold
		goldAmount = (score-(score%100))/100;
		score = score %100;
		var goldBurst = new ParticleSystem.Burst(0.0f,(short)goldAmount);
		goldEm.SetBurst(0,goldBurst);

		//setTin
		tinAmount = (score-(score%10))/10;
		score = score %10;	
		var tinBurst = new ParticleSystem.Burst(0.0f,(short)tinAmount);
		tinEm.SetBurst(0,tinBurst);

		//setCooper
		cooperAmount = score;
		var cooperBurst = new ParticleSystem.Burst(0.0f,(short)cooperAmount);
		cooperEm.SetBurst(0,cooperBurst);

		Instantiate(goldPS.gameObject,position,Quaternion.identity);
		Instantiate(tinPS.gameObject,position,Quaternion.identity);
		Instantiate(cooperPS.gameObject,position,Quaternion.identity);
	}

}
