using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour 
{
	public int index;
	public Text[] texts;

	public void SetTexts(string[] textsIn)
	{
		if(textsIn.Length != texts.Length)
		{
			Debug.LogError("FatalError please add as many arguments as texts size. Otherwhise the world will explose");
			return;
		}

		for (int i = 0; i < textsIn.Length; i++)
		{
			texts[i].text = textsIn[i]; 
		}
	}
}
