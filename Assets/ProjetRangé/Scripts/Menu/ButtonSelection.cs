using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelection : MonoBehaviour, ISelectHandler,IDeselectHandler {

	public CameraMenu cameraMenu;
	public int positionIndex;

    public void OnDeselect(BaseEventData eventData)
    {
		cameraMenu.originPosition = positionIndex;
    }

    public void OnSelect(BaseEventData eventData)
    {
		cameraMenu.SetActualPosition(positionIndex);
    }
}
