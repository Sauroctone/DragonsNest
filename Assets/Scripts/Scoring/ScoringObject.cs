using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringObject : MonoBehaviour {

	public int scoreAmount = 1;
	

	internal void SetScore()
	{
		GameManager.Instance.scoreManager.score += scoreAmount + GameManager.Instance.scoreManager.comboAmount ; 
	}

}
