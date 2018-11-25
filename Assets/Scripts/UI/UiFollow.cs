using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiFollow : MonoBehaviour {

	public Transform self;
	Camera mainCam;
	private Vector2 farFromEggDistance;
	public GUIStyle originStyle;
	GUIStyle actualStyle;

	private void Start()
	{
		mainCam = Camera.main;
		actualStyle = originStyle;

	}

	// Update is called once per frame
	void Update () {
		//UpdatePos();
	}

	private void OnGUI ()
	{
		GUICreatorEgg();
	}

	private void GUICreatorEgg ()
	{
		Vector3 screenPosition = mainCam.WorldToScreenPoint(self.position);
		screenPosition.y = Screen.height - screenPosition.y;
		
		farFromEggDistance.y =0;
		
		if(screenPosition.y<40)
		{
			farFromEggDistance.y = screenPosition.y -40;
			screenPosition.y=40;
		}


		if(screenPosition.y>Screen.height)
		{
			farFromEggDistance.y = screenPosition.y-Screen.height;
			screenPosition.y=Screen.height;
		} 


		if(screenPosition.x<20) 
		{
			farFromEggDistance.x = screenPosition.x;
			screenPosition.x=20;	
		}
		if(screenPosition.x>Screen.width-20)
		{
			farFromEggDistance.x = screenPosition.x;
			screenPosition.x= Screen.width;
		}

		Debug.Log(farFromEggDistance.y);
		GUI.backgroundColor = Color.blue;
 		GUI.Box(new Rect(screenPosition.x-20,screenPosition.y-60,40,40),"Oeuf",actualStyle);

	}
}
