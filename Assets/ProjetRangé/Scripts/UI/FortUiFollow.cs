 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FortUiFollow : MonoBehaviour {

	Transform self;
	public Transform player;

	[Range(0.1f,2.0f)]
	public float scaleEgg;
	private Camera mainCam;
	public Material fortShader;
	private float timeFact;
	public Color DefaultColor;
	private Color color;

	private void Awake()
	{
		player = GameManager.Instance.playerControllerInstance.transform;

		mainCam = Camera.main;
		self = transform;
		OnEnable();

	}

	private void OnEnable()
	{
		StartCoroutine(ReturnAlpha());
	}

	private void 
	OnGUI ()
	{
		GUICreatorFort();
	}

	private void GUICreatorFort ()
	{
		if(!DrawFort()) DrawIndicator();
	}

	private bool DrawFort ()
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

		fortShader.SetColor("_ColorTextFL",color);

		Graphics.DrawTexture(new Rect(screenPosition.x-12,screenPosition.y-52,25*scaleEgg,25*scaleEgg),tex,fortShader);
		
		return true;
	}

	private void DrawIndicator ()
	{
		Vector3 decalPlayer =  self.position - player.position;
		decalPlayer = new Vector3 (decalPlayer.x, -decalPlayer.z, decalPlayer.y);
		decalPlayer = decalPlayer.normalized;


		var tex = UsualFunction.MakeTex(50,50,Color.white);
		fortShader.SetColor("_ColorTextFL",color);
		
		Graphics.DrawTexture(new Rect((decalPlayer.x+1)/2*Screen.width-5,(decalPlayer.y+1)/2*Screen.height-5,20,20),tex,fortShader);



		//	GUI.backgroundColor = Color.Lerp(egg.fullLifeCol, egg.lowLifeCol,1-(egg.life/egg.maxLife));
		//	GUI.Box(new Rect((decalPlayer.x+1)/2*Screen.width-5,(decalPlayer.y+1)/2*Screen.height-5,20,20),"", indicatorStyle);
	}

	private IEnumerator ReturnAlpha ()
	{
		color = new Color(0,0,0,0);
		yield return new WaitForSeconds(0.2f);
		color = DefaultColor;
		yield return new WaitForSeconds(0.7f);

		StartCoroutine(ReturnAlpha());

		yield break;
	}
}
