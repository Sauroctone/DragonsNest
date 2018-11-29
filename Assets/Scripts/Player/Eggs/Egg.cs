using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : LivingBeing {
	
	//[Range(0.0f,1.0f)]
	//public float life = 1.0f;

	[Header("Hatch's Variables")]
	public float hatchingTimeMax = 30.0f;
	public float hatchingTime = 0.0f;

	public MeshRenderer rend;
	private Material material;
	public ParticleSystem particle;
    public GameObject pickupCol;
	public bool canBeADrone;

	public Color fullLifeCol = Color.green;
	public Color lowLifeCol = Color.red;
    SpawnManager spawnMan;
   
	public override void Start ()
    {
        base.Start();

        material = rend.material;
		canBeADrone = false;
        spawnMan = GameManager.Instance.spawnMan;
	}
	
	public override void Update ()
    {
        base.Update();
		//LifeUpdate();
		//ParticleUpdate();
		HatchUpdate();
	}

	void HatchUpdate()
	{
		if (hatchingTime >= hatchingTimeMax)
		{
            pickupCol.SetActive(true);
			canBeADrone = true;
		}
		else
		{
			hatchingTime += Time.deltaTime;
		}
	}

    void ParticleUpdate()
	{
		var em = particle.emission;
		em.rateOverTime = 500*(hatchingTime/hatchingTimeMax);
	}

    public void Hatch()
    {
        spawnMan.targets.Remove(transform);        
		gameObject.SetActive(false);

    }

    // Overrides

    public override void UpdateHealthUI(int _damage)
    {
        //material.color = new Color(1 - life/maxLife, life/maxLife, 0, 1);
    }
	
    public override void Die()
    {
        base.Die();

        spawnMan.targets.Remove(transform);
        gameObject.SetActive(false);
    }
}
