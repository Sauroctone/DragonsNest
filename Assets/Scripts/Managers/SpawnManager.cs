using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    public List<EnemyBehaviour> enemies = new List<EnemyBehaviour>();
    public int maxEnemyCount;
    public List<Transform> eggs = new List<Transform>();
    public List<Transform> spawners = new List<Transform>();
    Transform player;
    Rigidbody playerRb;

    public void Init()
    {
        player = GameManager.Instance.player.transform;
        playerRb = GameManager.Instance.playerRb;
    }

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

    public Transform GetRandomTarget()
    {
        Transform target = null;
        target = eggs[Mathf.FloorToInt(Random.Range(0, eggs.Count))];
        return target;
    }
}