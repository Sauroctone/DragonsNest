 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiFollow : MonoBehaviour {

	Transform self;
	public Transform player;
	public Egg egg;

	[Range(0.1f,2.0f)]
	public float scaleEgg;
	private Camera mainCam;
	private GUIStyle eggStyle;
	private GUIStyle indicatorStyle;
	public Material çavaetrefun;

	public Texture2D eggImage;
	public Texture2D indicatorImage;

	Color color;

	private void Awake()
	{
		mainCam = Camera.main;
		eggStyle = new GUIStyle();
		eggStyle.normal.background = eggImage;	

		self = transform;

		indicatorStyle = new GUIStyle ();
		indicatorStyle.normal.background = indicatorImage;	
		
	}

	private void OnGUI ()
	{
		GUICreatorEgg();
	}

	private void GUICreatorEgg ()
	{
		if(!DrawEggg()) DrawIndicator();
	}

	private bool DrawEggg ()
	{
		Vector3 screenPosition = mainCam.WorldToScreenPoint(self.position);
		screenPosition.y = Screen.height - screenPosition.y;

		if((screenPosition.y<0) ||
		(screenPosition.y>Screen.height))
			return false;

		if ((screenPosition.x<0) ||
		(screenPosition.x>Screen.width))
			return false;

		GUI.backgroundColor = Color.Lerp(egg.fullLifeCol, egg.lowLifeCol,1-(egg.life/egg.maxLife));
		//GUI.Box(new Rect(screenPosition.x-20,screenPosition.y-60,40*scaleEgg,40*scaleEgg),"",eggStyle);
		var tex = UsualFunction.MakeTex(50,50,Color.white);
		//	Graphics.DrawTexture(new Rect(0,0,50,50),tex,çavaetrefun);
		Graphics.DrawTexture(new Rect(screenPosition.x-20,screenPosition.y-60,40*scaleEgg,40*scaleEgg),tex,çavaetrefun);
		return true;
	}

	private void DrawIndicator ()
	{
		Vector3 decalPlayer =  self.position - player.position;
		decalPlayer = new Vector3 (decalPlayer.x, -decalPlayer.z, decalPlayer.y);
		decalPlayer = decalPlayer.normalized;

		GUI.backgroundColor = Color.Lerp(egg.fullLifeCol, egg.lowLifeCol,1-(egg.life/egg.maxLife));
		GUI.Box(new Rect((decalPlayer.x+1)/2*Screen.width-5,(decalPlayer.y+1)/2*Screen.height-5,20,20),"", indicatorStyle);
	}
}
