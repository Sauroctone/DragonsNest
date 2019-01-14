using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMenu : MonoBehaviour {
	[Range(1.0f,3.0f)]
	public float speed;
	public Transform[] positions;
	[System.NonSerialized]
	public bool firstSelected = false ;
	public OnlyKeyBoardInputModule inputModule;
	private float timer = 1;
	[System.NonSerialized]
	public int originPosition;
	private int actualPosition;

	// Use this for initialization
	void Start () {
		
	}

	public void SetFirstSelected()
	{
		firstSelected = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(timer<1)
		{
			UpdateCamPosition(positions[originPosition],positions[actualPosition]);
		}
	}

	public void SetActualPosition(int pos)
	{
		if(firstSelected)
		{
			timer = 0.0f;
			actualPosition = pos;
		}
		else
		{
			firstSelected = true;
		}
	}


	void UpdateCamPosition (Transform origin, Transform destination)
	{
		transform.position = Vector3.Lerp(origin.position,destination.position,timer);
		transform.rotation = Quaternion.Lerp(origin.rotation, destination.rotation, timer);
		timer += Time.deltaTime*speed;
	}
}
