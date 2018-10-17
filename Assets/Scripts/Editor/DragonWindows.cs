using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public enum DragonWindowsTabs {
	Main,
	Dragons,
}

public class DragonWindows : EditorWindow {
	private float height;
	private float width;

	private ScriptableData scriptableData;
	private List<Dragon> dragons;
	private Dragon actualDragon;

	private GUIStyle nameStyle;



	DragonWindowsTabs dwt = DragonWindowsTabs.Main; 

	[MenuItem("Window/DragonsManager")]
	public static void InitializeWindows ()
	{
		DragonWindows win = (DragonWindows)EditorWindow.GetWindow(typeof(DragonWindows));
		win.Show();
	}
	

	private void OnEnable (){
		if(Resources.Load("Data/Data") != null)
		{
			scriptableData = (ScriptableData)Resources.Load("Data/Data");
			Debug.Log("Data well charged");
		}
		else
		{
			Debug.LogError("There is no Data in Resources/Data");
		}
		dragons = new List<Dragon> ();

		dwt = DragonWindowsTabs.Main;

		nameStyle = new GUIStyle ();
		nameStyle.fontSize = 20;
		nameStyle.fontStyle = FontStyle.Bold;
		//nameStyle.normal.textColor = Color.white;
		nameStyle.alignment = TextAnchor.MiddleCenter;
	}

	private void OnGUI (){

		SetWindowsVar();

		switch (dwt)
		{
			case DragonWindowsTabs.Main :
				MainGUI();
				break;
				
			case DragonWindowsTabs.Dragons :
				DragonsGUI();
				break;
		}
	}

	float scroll;
	private void MainGUI (){
		if(GUI.Button(new Rect(10,10,110,27),"New Dragon...")){
			NewDragon();
		}
		//sscroll = GUI.VerticalScrollbar (new Rect(width-20,55,10,height-60),scroll,0.5f,0.0f,1.0f);
		GUILayout.BeginArea(new Rect(10, 55, width-20, height-20));
			if(dragons.Count>0){
					SetButtonPosition();
			}else {
				GUILayout.Label ("You don't have any dragons, please create one");
			}
		GUILayout.EndArea();		
	}

	private void DragonsGUI (){
			GUILayout.BeginArea(new Rect(10, 10, width-20, height-35));
				GUILayout.Label(actualDragon.name, nameStyle);
				actualDragon.name = EditorGUILayout.TextField ("Name :",actualDragon.name);
				actualDragon.icon = (Texture2D)EditorGUILayout.ObjectField ("icon : ",actualDragon.icon,typeof(Texture2D),false) as Texture2D;
			GUILayout.EndArea();
		DeleteButton();
		BackButton();
	}

	private void SetWindowsVar ()
	{
		height = position.height; 
		width = position.width; 
	}
	private void SetButtonPosition(){
		var buttonWidth = 75;
		var buttonHeight = 75;
		var textHeight = 25;
		var buttonInter = 10;

		Rect lastRect = new Rect (0,0,buttonWidth,buttonHeight);
		foreach (var dragon in dragons){
			DragonButton(lastRect, dragon, textHeight);
			if(lastRect.x + buttonWidth*2 + buttonInter < width)
			{
				lastRect = new Rect (lastRect.x + buttonWidth +buttonInter,lastRect.y,buttonWidth,buttonHeight);
			}
			else
			{
				lastRect = new Rect (0,lastRect.y+ buttonHeight + buttonInter + textHeight,buttonWidth,buttonHeight);
			}
		}
	}
	private void BackButton (){
		
		if(GUI.Button(new Rect(10,height-35,100,25),"← Back")){
			dwt = DragonWindowsTabs.Main;
		}
	}
	
	private void DeleteButton (){
		if(GUI.Button(new Rect(width-110,height-35,100,25),"Delete")){
			dragons.Remove(actualDragon);
			dwt = DragonWindowsTabs.Main;
		}
	}

	private void DragonButton (Rect rect, Dragon dragon, float textHeight){
		GUI.contentColor = Color.green;
		if (GUI.Button(rect,dragon.icon)){
			actualDragon = dragon;
			dwt = DragonWindowsTabs.Dragons;
		}
		GUI.contentColor = Color.white;
		GUI.Label(new Rect(rect.x,rect.y+rect.height+5,rect.width,textHeight),dragon.name);
	}

	private void NewDragon (){
		dragons.Add(new Dragon());
	}

}
