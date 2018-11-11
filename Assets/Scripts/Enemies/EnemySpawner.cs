using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public GameObject enemyPrefab;
    public float baseTimeToSpawn;
    public float minTimeToSpawn;
    public float spawnTimeDecrement;
    public float timeToDecrementSpawnTime;
    float timeToSpawn;

    [Header("References")]
    public SpawnManager spawnMan;

    private void Start()
    {
        timeToSpawn = baseTimeToSpawn;
        StartCoroutine(IDecrementSpawnTime());
        StartCoroutine(ISpawnEnemy());
    }

    IEnumerator ISpawnEnemy()
    {
        while (spawnMan.enemies.Count < spawnMan.maxEnemyCount)
        {
            spawnMan.SpawnEnemy(enemyPrefab, transform);
            yield return new WaitForSeconds(timeToSpawn);
        }
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
