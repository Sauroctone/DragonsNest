using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    public List<EnemyBehaviour> enemies = new List<EnemyBehaviour>();
    public int maxEnemyCount;
    public List<Transform> targets = new List<Transform>();
    public List<EnemySpawner> spawners = new List<EnemySpawner>();
    List<EnemySpawner> activeSpawners = new List<EnemySpawner>();

    [Header("Wave system")]
    public WaveState waveState;
    public float waveTimer;
    public float restTimer;
    int currentWave;
    public float spawnerIncrement;

    Transform player;
    Rigidbody playerRb;
    Coroutine waveCor;
    Coroutine restCor;

    public void Init()
    {
        player = GameManager.Instance.player.transform;
        playerRb = GameManager.Instance.playerRb;

        waveCor = StartCoroutine(IWave());
    }

    //Enemy methods

    public void SpawnEnemy(GameObject _enemyPrefab, Transform _spawner)
    {
        GameObject enemyInstance = Instantiate(_enemyPrefab, _spawner.position, Quaternion.identity);
        EnemyBehaviour enemy = enemyInstance.GetComponent<EnemyBehaviour>();
        enemies.Add(enemy);

        enemy.spawnMan = this;
        enemy.player = player;
        enemy.playerRb = playerRb;
        enemy.Init();
    }

    public void RemoveEnemyFromList(EnemyBehaviour enemy)
    {
        enemies.Remove(enemy);

        if (enemies.Count == 0 && waveState == WaveState.WAITING_FOR_LAST_ENEMIES)
        {
            EndWave();
        }
    }

    public Transform GetNewTarget(Vector3 _position)
    {
        Transform target = null;
        if (targets.Count > 0)
            target = targets[0];
        else
            target = player;

        foreach (Transform targ in targets)
        {
            if (Vector3.Distance(targ.position, _position) < Vector3.Distance(target.position, _position))
                target = targ;
        }

        return target;
    }

    public void AddTargetToList(Transform _newTarget)
    {
        targets.Add(_newTarget);
    }

    //Wave methods

    void ActivateRandomSpawners(int _count)
    {
        List<EnemySpawner> tempList = spawners;
        for (int i = 0; i < _count; i++)
        {
            int randomIndex = Mathf.FloorToInt(Random.Range(0, tempList.Count));
            EnemySpawner spawner = tempList[randomIndex];
            spawner.gameObject.SetActive(true);
            tempList.RemoveAt(randomIndex);
            activeSpawners.Add(spawner);
        }
        Debug.Log("Activated " + _count + " spawners");
    }

    void EndWave()
    {
        Debug.Log("End of wave " + currentWave + ", now resting");

        foreach (EnemySpawner spawner in activeSpawners)
        {
            spawner.Die();
        }

        if (waveCor != null)
            StopCoroutine(waveCor);
        
        restCor = StartCoroutine(IRest());
    }

    //Coroutines

    IEnumerator IWave()
    {
        waveState = WaveState.DURING_WAVE;
        currentWave++;
        Debug.Log("Launching wave " + currentWave);
        int spawnersToActivate = Mathf.CeilToInt(currentWave * spawnerIncrement);
        ActivateRandomSpawners(Mathf.Clamp(spawnersToActivate, 0, spawners.Count));
        yield return new WaitForSeconds(waveTimer);
        waveState = WaveState.WAITING_FOR_LAST_ENEMIES;
        Debug.Log("End of wave : waiting for last enemies");
    }

    IEnumerator IRest ()
    {
        waveState = WaveState.RESTING;
        yield return new WaitForSeconds(restTimer);
        waveCor = StartCoroutine(IWave());
    }
}