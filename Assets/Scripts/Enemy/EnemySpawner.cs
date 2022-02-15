using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    private int _currentWave = 0;
    private Wave _wave;
    private int _quantityWave;

    [Header("Parametrs")]
    [SerializeField]
    private List<Wave> _waves = new List<Wave>();
    [SerializeField]
    private float _minTimeWaitForNextSpawnEnemy;
    [SerializeField]
    private float _maxTimeWaitForNextSpawnEnemy;
    [SerializeField]
    private List<Enemy> _enemyList = new List<Enemy>();

    [Header("Game Manager")]
    [SerializeField]
    private GameManager _gameManager;

    public bool IsTheLastEnemyInTheLastWave {
        get {
            if (_enemyList.Count == 0 && _currentWave == _quantityWave) {
                return true;
            }

            return false;
        }
    }

    public bool IsLastWave {
        get {
            if (_currentWave == _quantityWave) {
                return true;
            }

            return false;
        }
    }

    public bool IsTheLastEnemyInWave {
        get {
            if (_enemyList.Count == 0) {
                return true;
            }

            return false;
        }
    }

    private void OnEnable() {
        _wave = _waves[0];
        _quantityWave = _waves.Count;
    }

    public void AddEnemy(Enemy enemy) {
        _enemyList.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy) {
        _enemyList.Remove(enemy);
    }

    public void EnemyWaveSpawn() {
        _currentWave++;
        _gameManager.UpdateWaveText(_currentWave, _waves.Count);

        for (int i = 0; i < _wave.spawns.Count; i++) {
            StartCoroutine(EnemySpawn(_wave.spawns[i]));
        }

        if (_currentWave < _waves.Count) {
            _wave = _waves[_currentWave];
        }
    }

    private IEnumerator EnemySpawn(Spawn spawn) {
        for (int i = 0; i < spawn.enemies.Count; i++) {
            yield return StartCoroutine(Spawn(spawn, spawn.enemies[i]));
        }
    }

    private IEnumerator Spawn(Spawn spawn, EnemySpawn enemyInSpawn) {
        for (int i = 0; i < enemyInSpawn.amount; i++) {
            Enemy _enemy = Instantiate(enemyInSpawn.enemy, spawn.spawnPoint.position, Quaternion.identity);
            //_enemy.name = "enemy " + i;
            AddEnemy(_enemy);
            _enemy.Init(_gameManager, this);
            _enemy.SetWayPoints(spawn.wayPoints);

            float _timeWaitForNextSpawnEnemy = Random.Range(_minTimeWaitForNextSpawnEnemy, _maxTimeWaitForNextSpawnEnemy);
            yield return new WaitForSeconds(_timeWaitForNextSpawnEnemy);
        }
    }
}
