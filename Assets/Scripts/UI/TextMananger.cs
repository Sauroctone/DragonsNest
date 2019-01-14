using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextMananger : MonoBehaviour {

	public List<string> entry;
	private string textString;
	private Text textComponenent;

	// Use this for initialization
	public void Start () {
		textComponenent = GetComponent<Text>();
	}
	
	// Update is called once per frame
	private void Update () 
	{
		UpdateText();
	}

	public void UpdateText()
	{
		textString = ConvertStringsToString(entry);
		if(textString != textComponenent.text)
		{
			textComponenent.text = textString;
		}
	}

	private string ConvertStringsToString (List<string> stringList)
	{
		if(stringList.Count <=0 || stringList == null){return "";}
		
		var _strFinal = "";
		
		foreach (var str in stringList)
		{
			_strFinal += " " + str;
		}
		return _strFinal;
	}

	private string ConvertStringsToString (List<string> stringList, char separator)
	{
		if(stringList.Count <=0 || stringList == null){return "";}
		
		var _strFinal = "";
		
		foreach (var str in stringList)
		{
			_strFinal += separator + str;
		}
		return _strFinal;
	}
}
