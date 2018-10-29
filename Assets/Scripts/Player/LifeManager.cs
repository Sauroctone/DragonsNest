using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour {
	public enum TpeOfLm
	{
		Player,
		Turret
	}
	public TpeOfLm typeOfObjec;
	public int lifeMax = 100; 
	int life = 100; 
	public Image lifeBar;
	
	void Start()
	{
		life = lifeMax;
	}
	void OnTriggerEnter(Collider col)
	{
		var proj = col.gameObject.GetComponent<Projectile>();
		if(proj)
		{

			LifeUpdate(proj.firePower);
			//Change with the pool die 
			Destroy(proj.gameObject);
		} 
	}
	

	void LifeUpdate(int damage) 
	{	
		life -= damage;
			switch(typeOfObjec)
			{
				case TpeOfLm.Player:
					LifeBarUpdate();
					break;
					
				case TpeOfLm.Turret:
					break;
			}
		if(life-damage <= 0)
		{
			life = 0;
			LifeNull();

		}
	}

	void LifeBarUpdate()
	{
		lifeBar.fillAmount = life/lifeMax;
	}

	void LifeNull()
	{
		switch(typeOfObjec)
		{
			case TpeOfLm.Player:
				Debug.Log("You Die");
				break;

			case TpeOfLm.Turret:
				Debug.Log("You turret die");
				break;
		}
	}
}
