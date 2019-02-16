using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour {

	public TextMananger timer;
	public TextMananger score;
	public TextMananger[] individualScore;


	
	// Use this for initialization
	void Start () 
	{
		timer.entry[1] = GameManager.Instance.timeMan.timer.ToString("0000");

		score.entry[0] = GameManager.Instance.scoreManager.score.ToString("000000");
		
		individualScore[0].entry[1] = GameManager.Instance.scoreManager.archerDeathCount.ToString("0000");
		individualScore[1].entry[1] = GameManager.Instance.scoreManager.warlockDeathCount.ToString("0000");
		individualScore[2].entry[1] = GameManager.Instance.scoreManager.ballistaDeathCount.ToString("0000");
		
	}
	
	
}
