 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : LivingBeing {
	
	//[Range(0.0f,1.0f)]
	//public float life = 1.0f;

	[Header("Hatch's Variables")]
	public float hatchingTimeMax = 30.0f;
	public float hatchingTime = 0.0f;

	private Material material;
	public ParticleSystem particle;
    public GameObject pickupCol;
	public bool canBeADrone;

	public Color fullLifeCol = Color.green;
	public Color lowLifeCol = Color.red;
    SpawnManager spawnMan;
    private AudioSource EggAudio;
    private bool isSFXPlayed;
    public AudioClip eggDestroy;
    public AudioClip eggHatching;

    public override void Start ()
    {
        base.Start();

        EggAudio = GetComponentInParent<AudioSource>();
		canBeADrone = false;
        spawnMan = GameManager.Instance.spawnMan;
        hatchingTime = 0f;
        isAlive = true;
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
			var scale = hatchingTime/hatchingTimeMax;
			transform.localScale = new Vector3(scale,scale,scale);
		}
	}

    void ParticleUpdate()
	{
		var em = particle.emission;
		em.rateOverTime = 500*(hatchingTime/hatchingTimeMax);
	}

    public void Hatch()
    {
        EggAudio.PlayOneShot(eggHatching);
        spawnMan.eggs.Remove(transform);
        gameObject.SetActive(false);
        canBeADrone = false;
        pickupCol.SetActive(false);
    }

    // Overrides

    public override void UpdateHealthUI(int _damage)
    {
        //material.color = new Color(1 - life/maxLife, life/maxLife, 0, 1);
    }
	
    public override void Die()
    {
        EggAudio.PlayOneShot(eggHatching);
        base.Die();
		hatchingTime = 0;
        spawnMan.eggs.Remove(transform);
		pickupCol.SetActive(false);
        gameObject.SetActive(false);
    }
}
