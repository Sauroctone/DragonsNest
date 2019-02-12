 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiFollow : MonoBehaviour {

	Transform self;
	private Transform player;
	public Egg egg;

	[Range(0.1f,2.0f)]
	public float scaleEgg;
	private Camera mainCam;
	public Material eggShader;
	public Color eggLifeMaxColor, eggLowLifeColor, eggBAckgroundColor;	
	public Material circleShader;
	public Color circleColor, circleBAckgroundColor;	
	

	public Texture2D eggImage;
	public Texture2D indicatorImage;

	Color color;

	private void Awake()
	{
		player = GameManager.Instance.playerControllerInstance.transform;

		mainCam = Camera.main;
		self = transform;
		eggShader.SetColor("_ColorBase",eggBAckgroundColor);
		eggShader.SetColor("_ColorTextFL",eggLifeMaxColor);
		eggShader.SetColor("_ColorTextLL",eggLowLifeColor);
		
		
		circleShader.SetColor("_ColorBase",circleBAckgroundColor);
	}

	private void 
	OnGUI ()
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

		var tex = UsualFunction.MakeTex(50,50,Color.white);
		eggShader.SetFloat("_FillAmount",egg.life/egg.maxLife);
		circleShader.SetFloat("_FillAmount",egg.hatchingTime/egg.hatchingTimeMax);
		Graphics.DrawTexture(new Rect(screenPosition.x-20,screenPosition.y-60,40*scaleEgg,40*scaleEgg),tex,circleShader);
		Graphics.DrawTexture(new Rect(screenPosition.x-12,screenPosition.y-52,25*scaleEgg,25*scaleEgg),tex,eggShader);
		
		return true;
	}

	private void DrawIndicator ()
	{
		Vector3 decalPlayer =  self.position - player.position;
		decalPlayer = new Vector3 (decalPlayer.x, -decalPlayer.z, decalPlayer.y);
		decalPlayer = decalPlayer.normalized;


		var tex = UsualFunction.MakeTex(50,50,Color.white);
		eggShader.SetFloat("_FillAmount",egg.life/egg.maxLife);
		circleShader.SetFloat("_FillAmount",egg.hatchingTime/egg.hatchingTimeMax);
		
		if (egg.hatchingTime >= egg.hatchingTimeMax)
		{

		}
		
		Graphics.DrawTexture(new Rect((decalPlayer.x+1)/2*Screen.width-10,(decalPlayer.y+1)/2*Screen.height-10,30,30),tex,circleShader);
		Graphics.DrawTexture(new Rect((decalPlayer.x+1)/2*Screen.width-5,(decalPlayer.y+1)/2*Screen.height-5,20,20),tex,eggShader);



		//	GUI.backgroundColor = Color.Lerp(egg.fullLifeCol, egg.lowLifeCol,1-(egg.life/egg.maxLife));
		//	GUI.Box(new Rect((decalPlayer.x+1)/2*Screen.width-5,(decalPlayer.y+1)/2*Screen.height-5,20,20),"", indicatorStyle);
	}
}
