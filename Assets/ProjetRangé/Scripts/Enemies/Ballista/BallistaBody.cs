using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallistaBody : LivingBeing {

    public float minHealthbarScale;

    public override void UpdateHealthUI(int _damage)
    {
        lifeBar.rectTransform.localScale = Vector3.Lerp(new Vector3(minHealthbarScale, minHealthbarScale, minHealthbarScale), Vector3.one, life / maxLife);
    }

    public override void Die()
    {
        base.Die();
        scoringObject.scoreAmount = 20;
        GameManager.Instance.scoreManager.ballistaDeathCount++;

        GameManager.Instance.scoreManager.SetCombo();
        Destroy(gameObject);
    }
}
