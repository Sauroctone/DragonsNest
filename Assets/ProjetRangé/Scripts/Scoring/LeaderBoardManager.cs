using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardManager : MonoBehaviour 
{
	public bool justConsulting;
	public ScoreBoard[] scoresDisplay;

    public LeaderBoard score;

    public void Start()
	{
		    for (int i = 0; i < scoresDisplay.Length; i++)
		    {

			    var _currentLB = score.GetLeaderBoard()[i];
			    scoresDisplay[i].SetTexts(new string[]{_currentLB.playerName,_currentLB.date,_currentLB.yearReached,_currentLB.goldAmount.ToString("000000")});
		    }
      
	}

}
