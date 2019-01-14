using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutomaticColorChange : MonoBehaviour {
	
	public MaskableGraphic coloredTarget;
	public MaskableGraphic thisColored;

	private void Start()
	{
		thisColored = this.GetComponent<MaskableGraphic>();
	}

	// Update is called once per frame
	private void Update () 
	{
		if(thisColored.color == coloredTarget.color) return;
		thisColored.color = coloredTarget.color;
	}
}
