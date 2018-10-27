using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eggs : MonoBehaviour {
	
	[Range(0.0f,1.0f)]
	public float life = 1.0f;
	public float hatchingTimeMax = 30.0f;
	public float hatchingTime = 0.0f;

	public MeshRenderer rend;
	private Material material;
	public ParticleSystem particle;

	// Use this for initialization
	void Start () {
		material = rend.material;
	}
	
	// Update is called once per frame
	void Update () {
		LifeUpdate();
		ParticleUpdate();
		HatchUpdate();
	}

	void HatchUpdate()
	{
		if (hatchingTime >= 30)
		{
			
		}
		else
		{
			hatchingTime += Time.deltaTime;
		}

	}
	void LifeUpdate ()
	{
		material.color = new Color (1-life,life,0,1);
	}

	void ParticleUpdate()
	{
		var em = particle.emission;
		em.rateOverTime = 500*(hatchingTime/hatchingTimeMax);


	}
}
