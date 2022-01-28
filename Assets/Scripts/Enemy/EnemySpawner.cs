using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    private float _timeWaitForNextSpawnEnemy;
    private int _currentWaveInt = 0;
    private Wave _currentWave;
    private int _quantityWave;

    [Header("Parametrs")]
    [SerializeField]
    private int _startLayerEnemy;
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
            if (_enemyList.Count == 0 && _currentWaveInt == _quantityWave) {
                return true;
            }

            return false;
        }
    }

    public bool IsLastWave {
        get {
            if (_currentWaveInt == _quantityWave) {
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
        _currentWave = _waves[0];
        _quantityWave = _waves.Count;
    }

    public void AddEnemy(Enemy enemy) {
        _enemyList.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy) {
        _enemyList.Remove(enemy);
    }

    public void StartEnemySpawn() {
        _currentWaveInt++;
        _gameManager.UpdateWaveText(_currentWaveInt, _waves.Count);

        for (int i = 0; i < _currentWave.spawn.Count; i++) {
            StartCoroutine(EnemySpawn(_currentWave.spawn[i], i));
        }

        if (_currentWaveInt < _waves.Count) {
            _currentWave = _waves[_currentWaveInt];
        }
    }

    private IEnumerator EnemySpawn(Spawn spawn, int number) {
        for (int i = 0; i < spawn.amountEnemy; i++) {
            GameObject _enemyObject = Instantiate(spawn.enemy, spawn.spawnPoint.position, Quaternion.identity);
            _enemyObject.name = "enemy " + number;
            Enemy _enemyScript = _enemyObject.GetComponent<Enemy>();
            AddEnemy(_enemyScript);
            _enemyScript.Initialization(_gameManager, this);
            _enemyScript.SetWayPoints(spawn.wayPoints);
            _startLayerEnemy--;

            _timeWaitForNextSpawnEnemy = Random.Range(_minTimeWaitForNextSpawnEnemy, _maxTimeWaitForNextSpawnEnemy);
            yield return new WaitForSeconds(_timeWaitForNextSpawnEnemy);
        }
    }
}
