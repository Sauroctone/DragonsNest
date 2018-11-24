using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using FDI;

public class FDInputWindows : EditorWindow 
{
	#region Winodws Var
		static float h = 270.0f;
		static float w = 480.0f;
	#endregion
	[MenuItem("FDInput/InputWindow")]
	private static void Init ()
	{
		var win = EditorWindow.GetWindow(typeof(FDInputWindows),true,"FDInputManager",true) as FDInputWindows;
		win.Show();
		win.maxSize = new Vector2(w,h); 
		win.minSize = new Vector2(w,h); 
		
	}

	#region FDI Var
		private List<FDI_Input> allInputs;
		private FDI_Input actualInput;
	#endregion
	private void OnEnable ()
	{

	}
	private void OnGUI()
	{
		if(actualInput == null) GUINoActualInput();
	}

	private void GUINoActualInput()
	{
		GUI.Label(new Rect(0,0,w,h),"Vous n'avez aucun Input d'entré dans le projet pour le moment");
	}


}
