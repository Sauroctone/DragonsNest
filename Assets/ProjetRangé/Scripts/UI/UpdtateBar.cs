using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UpdtateBar : MonoBehaviour {
	[System.NonSerialized]
	public float percent = 0.5f;
	private Image image;
	public UnityEvent OnFilledPermanent;
	public UnityEvent OnUnfilledPermanent;
	private bool onFilledEvent;
	private bool onUnfilledEvent;
	public UnityEvent OnFilledOnce;
	public UnityEvent OnUnfilledOnce;
	
	void Start () 
	{
		image = this.GetComponent<Image>();
	}

	private void Update ()
	{
		if(percent<=0)
		{
			DoEvent();
		}
		else 
		{
			if (onUnfilledEvent == true)
			{
				onUnfilledEvent = false;
			}
		}

		if(percent>=1)
		{
			DoEvent();
		}
		else 
		{	
			if (onFilledEvent == true)
			{
				onFilledEvent = false;
			}
		}
		
	}

	public void DoEvent ()
	{
		if(percent<=0)
		{
			OnUnfilledPermanent.Invoke();
			if(!onUnfilledEvent)
			{
				OnUnfilledOnce.Invoke();
				onUnfilledEvent = true;
			}
		}
		if(percent>=1)
		{
			OnFilledPermanent.Invoke();
			if(!onFilledEvent)
			{
				OnFilledOnce.Invoke();
				onFilledEvent = true;
			}
		}
	}

	public void UpdateFill (float amount) 
	{
		percent = amount;
		if(percent<0)
		{
			percent = 0;
		}
		if(percent>1)
		{
			percent = 1;
		}
		image.fillAmount = percent;
	}

}
