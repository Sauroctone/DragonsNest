using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : LivingBeing
{
    public float minHealthbarScale;

    [Header("Spawn")]
    public bool isOutOfMap;
    public GameObject enemyPrefab;
    public Transform spawnPosition;
    float timeBeforeSpawn;
    public float baseTimeToSpawn;
    public float minTimeToSpawn;
    public float waveCountDecrement;
    public float survivedCountDecrement;
    int wavesSurvived;
    public GameObject[] survivalFeedbackObjects;
    SpawnManager spawnMan;
    Coroutine spawnCor;
    [Header("References")]
    public Renderer rend;
    public Material normalMat;
    public Material invincibleMat;
    public GameObject canvas;

    //Overrides

    public override void Start()
    {
        base.Start();

        if (spawnMan == null)
            spawnMan = GameManager.Instance.spawnMan;

        if (isOutOfMap)
            MakeInvincible(0);
    }

    public void OnWaveBeginning()
    {
        rend.material = normalMat;
        if (canvas != null)
            canvas.SetActive(true);
        spawnCor = StartCoroutine(ISpawnEnemy());
    }

    public void OnWaveEnd()
    {
        if (spawnCor != null)
            StopCoroutine(spawnCor);

        if (!isOutOfMap)
            LevelUp();

        MakeInvincible(spawnMan.restTimer);
        ResetLife(0);
        if (canvas != null)
            canvas.SetActive(false);
    }

    void LevelUp()
    {
        if (survivalFeedbackObjects.Length > wavesSurvived+1)
        {
            survivalFeedbackObjects[wavesSurvived].SetActive(false);
            wavesSurvived++;
            survivalFeedbackObjects[wavesSurvived].SetActive(true);
        }
        else
        {
            wavesSurvived++;
        }
    }

    void ResetLevel()
    {
        if (survivalFeedbackObjects.Length > wavesSurvived)
            survivalFeedbackObjects[wavesSurvived].SetActive(false);
        wavesSurvived = 0;
        survivalFeedbackObjects[wavesSurvived].SetActive(true);
    }

    public void Enable()
    {
        gameObject.SetActive(true);
        ResetLife(0f);
        ResetLevel();

        if (spawnMan == null)
            spawnMan = GameManager.Instance.spawnMan;
    }

    //Overrides

    public override void ResetHealthUI(float _timeToRegen)
    {
        if (canvas != null)
        {
            lifeBar.rectTransform.localScale = Vector3.one;
            //FEEDBACK BAR
        }
    }

    public override void UpdateHealthUI(int _damage)
    {
        lifeBar.rectTransform.localScale = Vector3.Lerp(new Vector3(minHealthbarScale, minHealthbarScale, minHealthbarScale), Vector3.one, life / maxLife);
    }

    public override void MakeInvincible(float _time)
    {
        base.MakeInvincible(_time);
        rend.material = invincibleMat;
    }

    public override void Die()
    {
        base.Die();
        spawnMan.activeSpawners.Remove(this);
        spawnMan.spawnersInMap.Add(this);
        gameObject.SetActive(false);
    }

    //Coroutines

    IEnumerator ISpawnEnemy()
    {
        if (spawnMan.enemies.Count < spawnMan.maxEnemyCount)
            spawnMan.SpawnEnemy(enemyPrefab, spawnPosition.position);

        timeBeforeSpawn = baseTimeToSpawn - waveCountDecrement * spawnMan.currentWave;
        timeBeforeSpawn -= survivedCountDecrement * wavesSurvived;
        if (timeBeforeSpawn < minTimeToSpawn)
            timeBeforeSpawn = minTimeToSpawn;

        //Debug.Log("Time to spawn enemy is " + timeBeforeSpawn + " for " + this.gameObject.name);

        yield return new WaitForSeconds(timeBeforeSpawn);
        spawnCor = StartCoroutine(ISpawnEnemy());
    }

    //Obsolete
    //IEnumerator IDecrementSpawnTime()
    //{
    //    while (timeToSpawn > minTimeToSpawn)
    //    {
    //        yield return new WaitForSeconds(timeToDecrementSpawnTime);
    //        timeToSpawn -= spawnTimeDecrement;
    //        if (timeToSpawn < minTimeToSpawn)
    //            timeToSpawn = minTimeToSpawn;
    //    }
    //}
}