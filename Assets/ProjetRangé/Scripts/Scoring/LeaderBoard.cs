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

	
	public void SaveScore (int reference)
	{
		PlayerPrefs.SetString("name_"+reference,playerName);
		PlayerPrefs.SetString("date_"+reference,date);
		PlayerPrefs.SetString("year_"+reference,yearReached);
		
		PlayerPrefs.SetInt("gold_"+reference,goldAmount);

		PlayerPrefs.Save();
	}

	public void LoadScore(int reference)
	{
		
		playerName = PlayerPrefs.GetString("name_"+reference,playerName);
		date = PlayerPrefs.GetString("date_"+reference,date);
		yearReached = PlayerPrefs.GetString("year_"+reference,yearReached);
		
		goldAmount = PlayerPrefs.GetInt("gold_"+reference,goldAmount);
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

	public void SaveLeaderBoard()
	{
		for (int i = 0; i < _scores.Count; i++)
		{
			_scores[i].SaveScore(i);
		}
		Debug.Log("Save");

	}

	public void LoadLeaderBoard()
	{
		for (int i = 0; i < _scores.Count; i++)
		{
			_scores[i].LoadScore(i);
		}
		Debug.Log("Load");
	}

}


