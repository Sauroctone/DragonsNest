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
		SceneManager.LoadScene(1);
	}

	public void Quit()
	{
		Application.Quit();
	}

}
