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
    public Egg egg;
	PlayerController player;

    private void Start()
    {
        nestRend = GetComponent<MeshRenderer>();
        player = GameManager.Instance.player;
    }

    private void Update()
    {
        if (active == false)
        {
            if (nestRend.material != notActiveMat)
                nestRend.material = notActiveMat;
        }
        else 
            if (nestRend.material != activeMat)
                nestRend.material = activeMat;
    }
/*$
 */
	public void OnTriggerEnter(Collider col)
	{
        Debug.Log("Banane, kokok");
        //Debug.Log("enter");
		if(col.gameObject.tag == "Dragon")
		{   

            if(!egg.gameObject.activeSelf && player.eggMan.eggSlider.fillAmount >= 1)
            {
                active = true;
                player.canLand = true;
                player.nestScript = this;
                player.nestPosition = transform.position;
            }
		}
	}

	public void OnTriggerExit(Collider col)
	{
		if(col.gameObject.tag == "Dragon")
		{
            active = false;
			player.canLand = false;
			player.nestScript = null;
        }
	}

	public Transform Action()
	{
		if(egg.gameObject.activeInHierarchy==false)
		{
			egg.Start();
			egg.gameObject.SetActive(true);
            return egg.transform;
		}
        return null;
	}
}