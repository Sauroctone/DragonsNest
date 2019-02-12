using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dragon{

	public string name;
	public Texture2D icon;
	public Texture2D _tempIcon;

	void OnEnable(){
		_tempIcon = (Texture2D)Resources.Load("300ppi/UnknownIcon");
	}
	public Dragon (){
		name = "New Dragon";
		icon = _tempIcon;
	}

}
