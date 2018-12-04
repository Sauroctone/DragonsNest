using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    public List<EnemyBehaviour> enemies = new List<EnemyBehaviour>();
    public int maxEnemyCount;
    public List<Transform> targets = new List<Transform>();
    public List<EnemySpawner> spawnersInMap = new List<EnemySpawner>();
    public List<EnemySpawner> activeSpawners = new List<EnemySpawner>();
    public List<EnemySpawner> spawnersOutOfMap = new List<EnemySpawner>();

    [Header("Wave system")]
    public WaveState waveState;
    public float waveTimer;
    public float restTimer;
    public int currentWave;
    public float spawnerCountIncrement;

    Transform player;
    Rigidbody playerRb;
    Coroutine waveCor;
    Coroutine restCor;

    public void Init()
    {
        player = GameManager.Instance.player.transform;
        playerRb = GameManager.Instance.playerRb;
    }

    private void Update()
    {
        if (waveState == WaveState.GAME_START && Input.GetKeyDown(KeyCode.Return))
            BeginWave();
    }

    //Enemy methods

    public void SpawnEnemy(GameObject _enemyPrefab, Vector3 _pos)
    {
        GameObject enemyInstance = Instantiate(_enemyPrefab, _pos, Quaternion.identity);
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
            Rest();
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
        _count -= activeSpawners.Count;

        for (int i = 0; i < _count; i++)
        {
            int randomIndex = Mathf.FloorToInt(Random.Range(0, spawnersInMap.Count));
            EnemySpawner spawner = spawnersInMap[randomIndex];
            spawner.Enable();
            spawnersInMap.RemoveAt(randomIndex);
            activeSpawners.Add(spawner);
        }
        Debug.Log("Activated " + _count + " spawners ; current spawner count is " + activeSpawners.Count);
    }

    void BeginWave()
    {
        waveCor = StartCoroutine(IWave());
        waveState = WaveState.DURING_WAVE;
        currentWave++;
        Debug.Log("Launching wave " + currentWave);
        int spawnersToActivate = Mathf.CeilToInt(currentWave * spawnerCountIncrement);
        ActivateRandomSpawners(Mathf.Clamp(spawnersToActivate, 0, spawnersInMap.Count));

        foreach(EnemySpawner spawner in activeSpawners)
        {
            spawner.OnWaveBeginning();
        }
        foreach (EnemySpawner spawner in spawnersOutOfMap)
        {
            spawner.OnWaveBeginning();
        }
    }

    void EndWave()
    {
        Debug.Log("End of wave " + currentWave);

        foreach (EnemySpawner spawner in activeSpawners)
        {
            spawner.OnWaveEnd();
        }
        foreach (EnemySpawner spawner in spawnersOutOfMap)
        {
            spawner.OnWaveEnd();
        }

        if (waveCor != null)
            StopCoroutine(waveCor);
    }

    void Rest()
    {
        waveState = WaveState.RESTING;
        restCor = StartCoroutine(IRest());
        Debug.Log("Now resting");
    }

    //Coroutines

    IEnumerator IWave()
    {
        yield return new WaitForSeconds(waveTimer);
        waveState = WaveState.WAITING_FOR_LAST_ENEMIES;
        EndWave();
        Debug.Log("Waiting for last enemies");
    }

    IEnumerator IRest ()
    {
        yield return new WaitForSeconds(restTimer);
        BeginWave();
    }
}