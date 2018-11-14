using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nest : MonoBehaviour
{

    [Header("Variables")]
    //public PlayerController playerController;
    private MeshRenderer nestRend;
    public bool active = false;
    public GameObject content;
    //public bool getSomething = false;

    [Header("Materials")]
    public Material notActiveMat;
    public Material activeMat;

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
    /*public void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "Dragon")// && //!playerController.canLand
		{
			//playerController.canLand = true;
		}
	}*/
}