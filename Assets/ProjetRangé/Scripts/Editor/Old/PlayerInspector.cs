using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


/* 
public enum PIStates
{
	Classic,
	Custom
}
[CustomEditor(typeof(PlayerController))]
public class PlayerInspector : Editor {
	
	private PIStates pis;
	private ScriptableData scriptableData;
	private PlayerController player;
	private Level level;
	private string[] dragonsName;


	private void OnEnable()
	{	
		player = (PlayerController)target;
		if(Resources.Load("Data/Data") != null)
		{
			scriptableData = (ScriptableData)Resources.Load("Data/Data");
		}
		else
		{
			Debug.LogError("There is no Data in Resources/Data");
		}

	}

	public override void OnInspectorGUI()
	{
		pis = (PIStates)EditorGUILayout.EnumPopup(pis);
		switch (pis)
		{
			case PIStates.Classic :
				DrawDefaultInspector();
				break;
			
			case PIStates.Custom :
				CustomGUI();
				break;			
		}
	}

	private void CustomGUI ()
	{
		GetDragonsName();
		if(dragonsName != null || dragonsName.Length != 0)
		{
			player.dragonPopup = EditorGUILayout.Popup("Choose Your Dragon", player.dragonPopup, dragonsName);
			player.level = (int)(Level)EditorGUILayout.EnumPopup("Level : ",(Level)player.level);
		}
	}


	private void GetDragonsName()
	{
		dragonsName = new string[scriptableData.dragons.Count];

		for (int i = 0; i < dragonsName.Length; i++)
		{
			dragonsName[i] = scriptableData.dragons[i].name;
		}
	}


}

*/