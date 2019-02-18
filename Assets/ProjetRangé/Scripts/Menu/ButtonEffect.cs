using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonEffect : MonoBehaviour {

	public EventSystem eventSystem;

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
		GameManager.Instance.lb.AddANewPlayer("Roger",GameManager.Instance.timeMan.timer,GameManager.Instance.scoreManager.score);
		//
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

}
