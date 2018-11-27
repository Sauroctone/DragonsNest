using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(UiFollow))]
public class SetMultiplesVarForUI : Editor {

	SerializedProperty pc;
	UiFollow ui;
	void OnEnable()
	{
		ui = target as UiFollow;
		pc = serializedObject.FindProperty("player");
	}
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		if(GUILayout.Button("SetPlayerController"))
		{
			ui.player = FindObjectOfType<PlayerController>().transform;
			EditorUtility.SetDirty(ui);
		}

	}

}

