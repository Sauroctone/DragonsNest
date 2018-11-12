using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivingBeing : MonoBehaviour {

    [Header("Life")]

	public float maxLife = 100;
    public float life = 100;
    [HideInInspector]
    public float lostLifeBeforeDecay;
    float timeSinceLastDamage;
    public float timeToUpdateFeedbackBar;
    Coroutine feedbackCor;
    public float feedbackDecayTime;
    bool feedbackIsDecaying;
    public Image lifeBar;
    public Image lifeBarFeedback;

	
	public virtual void Start()
	{
		life = maxLife;
	}

	void OnTriggerStay(Collider col)
	{
		var proj = col.gameObject.GetComponent<Projectile>();

		if (proj)
		{
			UpdateLife(proj.firePower);
            
            //Change with the pool die 
            if (proj.destroyOnContact)
                Destroy(proj.gameObject);
		} 
	}

	public virtual void UpdateLife(int damage) 
	{	
		life -= damage;

		if (life <= 0)
		{
			life = 0;
			Die();
		}

        UpdateHealthUI(damage);
	}

	public virtual void UpdateHealthUI(int _damage)
	{
        timeSinceLastDamage = 0;
        lostLifeBeforeDecay += _damage;
        if (lifeBar.fillAmount == lifeBarFeedback.fillAmount || feedbackIsDecaying)
        {
            if (feedbackIsDecaying)
            {
                lifeBarFeedback.fillAmount = lifeBar.fillAmount;
                feedbackIsDecaying = false;
            }
            if (feedbackCor != null)
                StopCoroutine(feedbackCor);
            feedbackCor = StartCoroutine(IHealthBarFeedback());
        }
		lifeBar.fillAmount = life/maxLife;
	}

	public virtual void Die()
	{

	}

    IEnumerator IHealthBarFeedback()
    {
        while (timeSinceLastDamage < timeToUpdateFeedbackBar)
        {
            timeSinceLastDamage += Time.deltaTime;
            yield return null;
        }
        lostLifeBeforeDecay = 0;
        float decayingTime = 0;
        feedbackIsDecaying = true;
        while (decayingTime < feedbackDecayTime)
        {
            decayingTime += Time.deltaTime;
            lifeBarFeedback.fillAmount = Mathf.Lerp(lifeBarFeedback.fillAmount, life / maxLife, decayingTime / feedbackDecayTime); //à revoir
            yield return null;
        }
        feedbackIsDecaying = false;
    }
}
