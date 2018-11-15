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

    //Conditional hiiiide
    public float timeToUpdateFeedbackBar;
    Coroutine feedbackCor;
    public float feedbackDecayTime;
    bool feedbackIsDecaying;
    public Image lifeBar;
    public Image lifeBarFeedback;

    bool isInvincible;
    Coroutine invincibleCor;
    Coroutine regenCor;

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
        if (!isInvincible)
        {
		    life -= damage;

		    if (life <= 0)
		    {
			    life = 0;
			    Die();
		    }

            UpdateHealthUI(damage);
        }
	}

	public virtual void UpdateHealthUI(int _damage)
	{
        if (regenCor != null)
            StopCoroutine(regenCor);

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

    public virtual void ResetLife(float _timeToRegenUI)
    {
        life = maxLife;
        ResetHealthUI(_timeToRegenUI);
    }

    public virtual void ResetHealthUI(float _timeToRegen)
    {
        if (feedbackCor != null)
            StopCoroutine(feedbackCor);
        feedbackIsDecaying = false;
        lostLifeBeforeDecay = 0;

        if (regenCor != null)
            StopCoroutine(regenCor);
        regenCor = StartCoroutine(IHealthBarRegen(_timeToRegen));
    }

    public virtual void MakeInvincible(float _time)
    {
        if (invincibleCor != null)
            StopCoroutine(invincibleCor);
        invincibleCor = StartCoroutine(IInvulnerability(_time));
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

    IEnumerator IHealthBarRegen(float _timeToRegen)
    {
        float time = 0;
        while (time < _timeToRegen)
        {
            time += Time.deltaTime;
            lifeBar.fillAmount = Mathf.Lerp(lifeBar.fillAmount, life / maxLife, time / _timeToRegen);
            lifeBarFeedback.fillAmount = Mathf.Lerp(lifeBarFeedback.fillAmount, life / maxLife, time / _timeToRegen);
            yield return null;
        }
        lifeBar.fillAmount = 1;
        lifeBarFeedback.fillAmount = 1;
    }

    IEnumerator IInvulnerability(float _time)
    {
        isInvincible = true;
        yield return new WaitForSeconds(_time);
        isInvincible = false;
    }
}
