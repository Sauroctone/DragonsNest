using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterManager : MonoBehaviour 
{
	public static ParameterManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

	public float playerSpeed;
	public float enemySpeed;
	public float eggSpeed;
	public int fortNumer;

}
