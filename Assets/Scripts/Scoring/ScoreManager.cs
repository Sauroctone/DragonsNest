using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreManager {

	public int score;
	public int comboAmount = 0;

	private float comboCoolDown;
	public float[] comboCoolDownValue = {0.0f,5.0f,4.0f,3.0f,2.0f,1.5f};

	public void DecreaseCombo()
	{
		if(comboAmount == 0) return;
		
		comboCoolDown += Time.deltaTime;
		if(comboCoolDown>=comboCoolDownValue[comboAmount])
		{
			comboAmount = 0;
			comboCoolDown = 0;
		}
	}

	public void SetCombo ()
	{
		comboCoolDown =0;

		if(comboAmount<=5)
		{
			comboAmount ++;
		}
	}
}
