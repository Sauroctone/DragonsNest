using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonEffect : MonoBehaviour {

	public EventSystem eventSystem;
	public InputField iF;
    [Header("Menu Sound System References")]
    private AudioSource[] cameraAudioSources;
    private AudioSource MusicSource;
    private AudioSource SfxSource;
    public AudioClip buttonMoveSFX;
    public AudioClip buttonConfirmSFX;
    public AudioClip titleSFX;
    private Scene activeScene;
    private bool isTitleSFXPlayed = false;

    private void Start()
    {
        activeScene = SceneManager.GetActiveScene();
        if (activeScene.name == "Menu")
        {
            cameraAudioSources = Camera.main.GetComponents<AudioSource>();
            MusicSource = cameraAudioSources[0];
            SfxSource = cameraAudioSources[1];
        }
    }

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
        SfxSource.PlayOneShot(buttonMoveSFX,1f);
	}

	public void LoadFirstScene()
	{
        if (isTitleSFXPlayed == false)
        {
            MusicSource.Stop();
            SfxSource.PlayOneShot(titleSFX, 1f);
            isTitleSFXPlayed = true;
            Invoke("LoadFirstScene", 1.2f);
        }
        else
        {
            SceneManager.LoadScene(3);
        }
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
        SfxSource.PlayOneShot(buttonConfirmSFX, 1f);
        SceneManager.LoadScene(2);
	}

	public void LoadMenu()
	{
        SfxSource.PlayOneShot(buttonConfirmSFX, 1f);
		SceneManager.LoadScene(0);
	}

	public void Quit()
	{
		Application.Quit();
        SfxSource.PlayOneShot(buttonMoveSFX, 1f);
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
