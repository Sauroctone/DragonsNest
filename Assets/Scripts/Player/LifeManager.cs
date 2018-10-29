using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour {
	public enum TypeOfLifeMan
	{
		Player,
		Turret
	}
	public TypeOfLifeMan typeOfObject;
	public int lifeMax = 100; 
	int life = 100; 
	public Image lifeBar;
	
	void Start()
	{
		life = lifeMax;
	}
	void OnTriggerEnter(Collider col)
	{
		var proj = col.gameObject.GetComponent<ProjectileCollision>();
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
			switch(typeOfObject)
			{
				case TypeOfLifeMan.Player:
					LifeBarUpdate();
					break;
					
				case TypeOfLifeMan.Turret:
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
		switch(typeOfObject)
		{
			case TypeOfLifeMan.Player:
				Debug.Log("You Die");
				break;

			case TypeOfLifeMan.Turret:
				Debug.Log("You turret die");
				break;
		}
	}
}
