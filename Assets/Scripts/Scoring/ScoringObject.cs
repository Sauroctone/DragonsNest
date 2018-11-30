using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringObject : MonoBehaviour {

	public int scoreAmount;
	internal int multiplicator;
	public int comboAmount;



	internal void SetMultiplicator()
	{
		multiplicator = 1;
	}

	internal void SetModifiyersAndApply()
	{

		SetMultiplicator();
		SetScore();
	}
	internal void SetScore()
	{
		GameManager.Instance.score += scoreAmount*multiplicator; 
	}

}
