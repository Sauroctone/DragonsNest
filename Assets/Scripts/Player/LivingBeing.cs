using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivingBeing : MonoBehaviour {

    [Header("Life")]
    public float maxLife = 100;
    public float life;
    [HideInInspector]
    public float lostLifeBeforeDecay;
    internal float timeSinceLastDamage;
    public bool isAlive = true;
    bool burntThisFrame;

    //Conditional hiiiide
    public float timeToUpdateFeedbackBar;
    internal Coroutine feedbackCor;
    public float feedbackDecayTime;
    internal bool feedbackIsDecaying;
    public Image lifeBar;
    public Image lifeBarFeedback;
    
    bool isInvincible;
    Coroutine invincibleCor;
    internal Coroutine regenCor;

    [Header("Score")]

    public ScoringObject scoringObject;

#region virtual
	public virtual void Start()
	{
		life = maxLife;
	}

	public virtual void OnTriggerStay(Collider col)
	{
		var proj = col.gameObject.GetComponent<Projectile>();

		if (proj)
		{
            //One fire damage tick overall per frame - bool is flagged false in update
            if (proj.isFire)
                if (burntThisFrame)
                    return;
                else
                    burntThisFrame = true;

			UpdateLife(proj.firePower);
            
            //Change with the pool die 
            if (proj.destroyOnContact)
                Destroy(proj.gameObject);
		} 
	}

    public virtual void Update()
    {
        burntThisFrame = false;
    }
    
	public virtual void UpdateLife(int damage) 
	{	
        if (!isInvincible && isAlive)
        {
		    life -= damage;

            UpdateHealthUI(damage);

            if (life <= 0)
		    {
			    life = 0;
			    Die();
		    }
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
        if(scoringObject != null)
        {
            scoringObject.SetScore();
        }
        isAlive = false;
	}

    public virtual void ResetLife(float _timeToRegenUI)
    {
        life = maxLife;
        ResetHealthUI(_timeToRegenUI);
        isAlive = true;
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

        if (_time == -1)
            isInvincible = true;
        else if (_time == 0)
            isInvincible = false;
        else
            invincibleCor = StartCoroutine(IInvulnerability(_time));
    }
#endregion
    internal virtual IEnumerator IHealthBarFeedback()
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

    internal virtual IEnumerator IHealthBarRegen(float _timeToRegen)
    {
        yield return new WaitForSeconds(1f);

        float time = 0;
        lifeBarFeedback.fillAmount = lifeBar.fillAmount;
        while (time < _timeToRegen)
        {
            time += Time.deltaTime;
            lifeBar.fillAmount = Mathf.Lerp(lifeBar.fillAmount, 1, time / _timeToRegen);
            lifeBarFeedback.fillAmount = Mathf.Lerp(lifeBarFeedback.fillAmount, 1, time / _timeToRegen);
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