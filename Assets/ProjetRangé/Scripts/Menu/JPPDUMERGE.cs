using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JPPDUMERGE : MonoBehaviour {

    
    public Button wesh;
	void Start ()
    {
        if(GameManager.Instance == null)
        {
            Debug.Log("wesh");
            wesh.interactable = false;
        }  
        else
        {
            wesh.interactable = true;
        }
    }
	
}
