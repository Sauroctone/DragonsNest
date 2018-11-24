using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : LivingBeing {
	
	//[Range(0.0f,1.0f)]
	//public float life = 1.0f;
	public float hatchingTimeMax = 30.0f;
	public float hatchingTime = 0.0f;

	public MeshRenderer rend;
	private Material material;
	public ParticleSystem particle;
    public GameObject pickupCol;
    SpawnManager spawnMan;
   
	public override void Start ()
    {
        base.Start();

        material = rend.material;
        spawnMan = GameManager.Instance.spawnMan;
	}
	
	public override void Update ()
    {
        base.Update();
		//LifeUpdate();
		ParticleUpdate();
		HatchUpdate();
	}

	void HatchUpdate()
	{
		if (hatchingTime >= hatchingTimeMax)
		{
            //TransformTurret();
            pickupCol.SetActive(true);
		}
		else
		{
			hatchingTime += Time.deltaTime;
		}
	}

	//void TransformTurret()
	//{
	//	turret.SetActive(true);
	//	this.gameObject.SetActive(false);
	//}

    //void LifeUpdate ()
    //{
    //	material.color = new Color (1-life,life,0,1);
    //}

    void ParticleUpdate()
	{
		var em = particle.emission;
		em.rateOverTime = 500*(hatchingTime/hatchingTimeMax);
	}

    public void Hatch()
    {
        spawnMan.targets.Remove(transform);
        Destroy(gameObject);
    }

    // Overrides

    public override void UpdateHealthUI(int _damage)
    {
        material.color = new Color(1 - life/maxLife, life/maxLife, 0, 1);
    }

    public override void Die()
    {
        base.Die();

        spawnMan.targets.Remove(transform);
        Destroy(gameObject);
    }
}
