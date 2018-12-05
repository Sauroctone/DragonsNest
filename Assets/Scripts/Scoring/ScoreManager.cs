using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ScoreManager : MonoBehaviour {

	public int score;
	public int comboAmount = 0;
	private float comboCoolDown;

	public Image comboCoolDownImage;
	public Text comboAmountText;
	public Text scoreText; 
	public float[] comboCoolDownValue = {0.0f,5.0f,4.0f,3.0f,2.0f,1.5f,1.5f,1.5f,1.5f};
	public Color[] comboColor = {Color.grey, Color.green,Color.yellow,Color.magenta,Color.red,Color.blue,Color.blue,Color.blue,Color.blue}; 


	public void Update()
	{
		DecreaseCombo();
		SetScore();
	}

	public void SetScore()
	{
		scoreText.text = score.ToString("00000");
	}

	public void DecreaseCombo()
	{
		if(comboAmount == 0){
			if(comboCoolDownImage.fillAmount != 1) 
			{
				comboCoolDownImage.fillAmount = 1;
				comboCoolDownImage.color = comboColor[comboAmount];
				comboAmountText.color = comboColor[comboAmount];
			}
			if(comboAmountText.text != "1") {comboAmountText.text = (comboAmount+1).ToString();}
			return;
		} 
		
		comboCoolDown += Time.deltaTime;
		comboCoolDownImage.fillAmount = 1-comboCoolDown/comboCoolDownValue[comboAmount];
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
			comboAmountText.text = (comboAmount+1).ToString();
			comboCoolDownImage.color = comboColor[comboAmount];
			comboAmountText.color = comboColor[comboAmount];
		}
	}
}
