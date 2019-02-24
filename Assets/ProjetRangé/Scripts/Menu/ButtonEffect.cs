using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonEffect : MonoBehaviour {

	public EventSystem eventSystem;
	public InputField iF;

	public void DisplayUI (GameObject go)
	{
		go.SetActive(true);
	}

	public void DisableUI (GameObject go)
	{
		go.SetActive(false);
	}

	public void SetNextButtonSelected (GameObject go)
	{
		eventSystem.SetSelectedGameObject(go);
	}

	public void LoadFirstScene()
	{
		SceneManager.LoadScene(3);
	}
	public void LoadLeaderBoardScene()
	{
		//TestPlease Dont toutch
		GameManager.Instance.lb.AddANewPlayer(iF.text,GameManager.Instance.timeMan.timer,Mathf.RoundToInt (GameManager.Instance.scoreManager.score*GameManager.Instance.paraMan.enemySpeed*(1+GameManager.Instance.paraMan.fortNumer*0.2f)/(GameManager.Instance.paraMan.eggSpeed*GameManager.Instance.paraMan.playerSpeed)));
		GameManager.Instance.lb.SaveLeaderBoard();
		//
		SceneManager.LoadScene(2);
	}
	public void LoadLeaderBoardSceneWithoutScore()
	{
		SceneManager.LoadScene(2);
	}

	public void LoadMenu()
	{
		SceneManager.LoadScene(0);
	}

	public void Quit()
	{
		Application.Quit();
	}

	public void SetDragonSpeed(float Speed)
	{
		ParameterManager.Instance.playerSpeed = Speed;
	}
	public void SetEggSpeed(float Speed)
	{
		ParameterManager.Instance.eggSpeed = Speed;
	}
	public void SetEnnemySpeed(float Speed)
	{
		ParameterManager.Instance.enemySpeed = Speed;
	}
	public void SetFortNumber(int Number)
	{
		ParameterManager.Instance.fortNumer = Number;
	}

}
