using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : LivingBeing
{
    public GameObject enemyPrefab;
    public float baseTimeToSpawn;
    public float minTimeToSpawn;
    public float spawnTimeDecrement;
    public float timeToDecrementSpawnTime;
    float timeToSpawn;
    SpawnManager spawnMan;

    //Overrides

    public override void Start()
    {
        base.Start();

        timeToSpawn = baseTimeToSpawn;
        spawnMan = GameManager.Instance.spawnMan;

        StartCoroutine(IDecrementSpawnTime());
        StartCoroutine(ISpawnEnemy());
    }

    public override void UpdateHealthUI(int _damage)
    {
        // NO HEALTHBAR (yet)
    }

    public override void Die()
    {
        base.Die();
        spawnMan.spawners.Remove(transform);
        Destroy(gameObject);
    }

    //Coroutines

    IEnumerator ISpawnEnemy()
    {
        if (spawnMan.enemies.Count < spawnMan.maxEnemyCount)
            spawnMan.SpawnEnemy(enemyPrefab, transform);
        yield return new WaitForSeconds(timeToSpawn);
        StartCoroutine(ISpawnEnemy());
    }

    IEnumerator IDecrementSpawnTime()
    {
        while (timeToSpawn > minTimeToSpawn)
        {
            yield return new WaitForSeconds(timeToDecrementSpawnTime);
            timeToSpawn -= spawnTimeDecrement;
            if (timeToSpawn < minTimeToSpawn)
                timeToSpawn = minTimeToSpawn;
        }
    }
}