using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eggs : MonoBehaviour {
	
	[Range(0.0f,1.0f)]
	public float lifeRate = 1.0f;
	public int lifeMax;
	int life;
	public float hatchingTimeMax = 30.0f;
	public float hatchingTime = 0.0f;

	public MeshRenderer rend;
	private Material material;
	public ParticleSystem particle;
	public GameObject turret;
	public bool canBeADrone;

	// Use this for initialization
	public void Start () {
		material = rend.material;
		hatchingTime =0;
		life = lifeMax;
	}
	
	// Update is called once per frame
	void Update () {
		LifeUpdate();
		ParticleUpdate();
		HatchUpdate();
	}

	void HatchUpdate()
	{
		if (hatchingTime >= hatchingTimeMax)
		{
			//TransformTurret();
			canBeADrone = true;
		}
		else
		{
			hatchingTime += Time.deltaTime;
		}

	}

	public void TransformDrone(PlayerController pc)
	{
		pc.babyDragonMan.SpawnNewBabyDragon();
		gameObject.SetActive(false);
	}

	void TransformTurret()
	{
		turret.SetActive(true);
		this.gameObject.SetActive(false);
	}
	void LifeUpdate ()
	{
		material.color = new Color (1-lifeRate,lifeRate,0,1);
	}

	void ParticleUpdate()
	{
		var em = particle.emission;
		em.rateOverTime = 200*(hatchingTime/hatchingTimeMax);
		
	}
}
