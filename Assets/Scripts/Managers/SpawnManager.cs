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

    public Transform GetNewTarget(Vector3 _position)
    {
        Transform target = null;
        if (eggs.Count > 0)
            target = eggs[0];
        foreach (Transform egg in eggs)
        {
            if (Vector3.Distance(egg.position, _position) < Vector3.Distance(target.position, _position))
                target = egg;
        }
        return target;
    }
}