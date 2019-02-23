using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeselectButton : MonoBehaviour 
{

public Button[] buttonToDeDeselect;

	public void DeselectingButton(Button target)
	{
		foreach (var item in buttonToDeDeselect)
		{
			item.interactable = true;
		}
		
		target.interactable = false;
	}

}
