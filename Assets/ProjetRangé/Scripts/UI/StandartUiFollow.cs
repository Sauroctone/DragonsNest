 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StandartUiFollow : MonoBehaviour {

	Transform self;

	[Range(0.1f,2.0f)]
	public float scale;
	private Camera mainCam;
	public Material shader;

	//Display an UI (create with unlit/transparent) above the object 

	private void Awake()
	{
		mainCam = Camera.main;
		self = transform;
	}

	private void 
	OnGUI ()
	{
		GUICreatorFort();
	}

	private void GUICreatorFort ()
	{
		DrawUI();
	}

	private bool DrawUI ()
	{
		Vector3 screenPosition = mainCam.WorldToScreenPoint(self.position);
		screenPosition.y = Screen.height - screenPosition.y;

		if((screenPosition.y<0) ||
		(screenPosition.y>Screen.height))
			return false;

		if ((screenPosition.x<0) ||
		(screenPosition.x>Screen.width))
			return false;

		var tex = UsualFunction.MakeTex(50,50,Color.white);

		Graphics.DrawTexture(new Rect(screenPosition.x-12,screenPosition.y-52,25*scale,25*scale),tex,shader);
		
		return true;
	}
}
