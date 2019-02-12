// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEditor;

// [CustomEditor(typeof(ScriptableData))]
// public class DataInspector : Editor {

// 	private bool debugMode = false;
// 	public override void OnInspectorGUI(){
// 		if(debugMode)
// 		{
// 			DrawDefaultInspector();
// 		}
// 		else
// 		{
// 			EditorGUILayout.LabelField("DragonsManager");
// 			if(GUILayout.Button("Open Editor"))
// 			{
// 				DragonWindows.InitializeWindows();
// 			}
// 		}

// 		debugMode = EditorGUILayout.Toggle("Debug Mode", debugMode);
// 	}

// }
