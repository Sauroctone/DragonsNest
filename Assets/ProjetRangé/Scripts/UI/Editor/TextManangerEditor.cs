using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TextMananger))]
public class TextManangerEditor : Editor {

	private TextMananger textMananger;

    private void OnEnable ()
	{
		textMananger = target as TextMananger;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		if(GUI.changed)
		{
			textMananger.Start();
			textMananger.UpdateText();
		}
	}

}
