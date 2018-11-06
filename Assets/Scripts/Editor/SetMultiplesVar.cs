using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(Nest))]
public class SetMultiplesVar : Editor {

	SerializedProperty pc;
	Nest nest;
	void OnEnable()
	{
		nest = target as Nest;
		pc = serializedObject.FindProperty("playerController");
	}
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		if(GUILayout.Button("SetPlayerController"))
		{
			nest.playerController = FindObjectOfType<PlayerController>();
		}

	}

}

