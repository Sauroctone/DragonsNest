using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Score
{
	public string playerName = "??????";
	public string date = "??????";
	public string yearReached = "????";
	public int goldAmount = 0;
	
	public Score (string PlayerName, int TimePlayed, int GoldAmount)
	{
		playerName = PlayerName;

		System.DateTime thisDay = System.DateTime.Today;
		date = thisDay.ToString("d");
		
		yearReached = TimePlayed.ToString("0000") + " Years";
		
		goldAmount = GoldAmount;
	}

	public Score ()
	{
		playerName = "??????";
		date = "??????";
		yearReached = "????";
		goldAmount = 0;
	}
}

[CreateAssetMenu(fileName = "LeaderBoard", menuName = "LB", order = 12)]
public class LeaderBoard : ScriptableObject
{
	public int maxLeaderBoard;
	public List<Score> _scores;

	public void AddANewPlayer (string playerName, int timePlayed, int goldAmount)
	{
		var tempScore = new Score(playerName,timePlayed,goldAmount);
		var index = FindIndexOfElementBelow(_scores,tempScore);

		Debug.Log(index);

		if(index < maxLeaderBoard)
		{
			_scores.Insert(index, tempScore);
			while(_scores.Count>maxLeaderBoard)
			{
				_scores.RemoveAt(maxLeaderBoard);
			}
		}
	}

	public List<Score> GetLeaderBoard ()
	{
		return _scores;
	}

	public int FindIndexOfElementBelow (List<Score> list, Score element) 
	{
		for (int i = 0; i < list.Count; i++)
		{
			if(list[i].goldAmount<element.goldAmount)
			{
				return i;
			}
		}
		return maxLeaderBoard+1;
	}
	public void ResetLeaderBoard()
	{
		if(_scores.Count == 0 || _scores == null)
		{
			_scores = new List<Score>();
			for (int i = 0; i < maxLeaderBoard; i++)
			{
				_scores.Add(new Score());
			}
		}
	}
}


