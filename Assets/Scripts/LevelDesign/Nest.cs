using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nest : MonoBehaviour
{

    [Header("Variables")]
    //public PlayerController playerController;
    private MeshRenderer nestRend;
    public bool active = false;
    //public bool getSomething = false;

    [Header("Materials")]
    public Material notActiveMat;
    public Material activeMat;
    
    [Header("References")]
    public Egg eggs;
	public PlayerController playerController;

    private void Start()
    {
        nestRend = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (active == false)
        {
            if (nestRend.material != notActiveMat)
            {
                nestRend.material = notActiveMat;
            }
        }
        else
        {
            if (nestRend.material != activeMat)
            {
                nestRend.material = activeMat;
            }
        }
    }

	public void OnTriggerEnter(Collider col)
	{
        Debug.Log("enter");
		if(col.gameObject.tag == "Dragon")
		{   
            active = true;
            if (eggs.canBeADrone)
            {
                eggs.Hatch();
            }
            if(!playerController.canLand)
            {
                playerController.canLand = true;
                playerController.nestScript = this;
                playerController.nestPosition = transform.position;
            }
		}
	}

	public void OnTriggerExit(Collider col)
	{
		if(col.gameObject.tag == "Dragon" && playerController.canLand)
		{
            active = false;
			playerController.canLand = false;
			playerController.nestScript = null;
		}
	}

	public Transform Action()
	{
		if(eggs.gameObject.activeInHierarchy==false)
		{
			eggs.Start();
			eggs.gameObject.SetActive(true);
            return eggs.transform;
		}
        return null;
	}
}