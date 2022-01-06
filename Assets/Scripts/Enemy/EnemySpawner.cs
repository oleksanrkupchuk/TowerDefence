﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private float _timeWaitForNextSpawnEnemy;
    private int _currentWave = 0;
    private DataWayPoints _currentWay = new DataWayPoints();
    private Vector2 _pointSpawn;

    [Header("Parametrs")]
    [SerializeField] 
    private GameObject enemyPrefab;
    [SerializeField] 
    private int _enemyAmountInWave;
    [SerializeField]
    private int _enemyAmountSpawn;
    [SerializeField] 
    private int _quantityWave;
    public int CurrentWave { get => _currentWave; }
    [SerializeField]
    private int _startLayerEnemy;
    [SerializeField]
    private List<DataWayPoints> _dataWaysPoints = new List<DataWayPoints>();
    [SerializeField]
    private float _minTimeWaitForNextSpawnEnemy;
    [SerializeField]
    private float _maxTimeWaitForNextSpawnEnemy;
    [SerializeField]
    private List<Enemy> _enemyList = new List<Enemy>();

    [Header("Game Manager")]
    [SerializeField]
    private GameManager _gameManager;

    public int maxLayerEnemy;
    public int minLayerEnemy;

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
            if(_currentWave == _quantityWave) {
                return true;
            }

            return false;
        }
    }

    public bool IsTheLastEnemyInWave {
        get {
            if(_enemyList.Count == 0) {
                return true;
            }

            return false;
        }
    }

    private void OnEnable() {
        Enemy.EnemySpawnerTest += RemoveEnemy2;
    }

    public void AddEnemy(Enemy enemy) {
        _enemyList.Add(enemy);
    }

    public void RemoveEnemy2(Enemy enemy) {
        print("remove enemy " + enemy.name);
    }

    public void RemoveEnemy(Enemy enemy) {
        _enemyList.Remove(enemy);
    }

    public void ResetMaxLayer() {
        maxLayerEnemy = 50;
    }

    public void ResetMinLayer() {
        maxLayerEnemy = 1;
    }

    public void StartSpawn() {
        StartCoroutine(EnemySpawn());
    }

    private IEnumerator EnemySpawn() {
        if(_currentWave < _quantityWave) {
            _currentWave++;
            _gameManager.UpdateWaveText(_currentWave, _quantityWave);
            ChooseEnemyWay();
            SetPointSpawn();
            for (int i = 0; i < _enemyAmountInWave; i++) {
                enemyPrefab.name = "enemy " + i;
                GameObject _enemyObject = Instantiate(enemyPrefab, _pointSpawn, Quaternion.identity);
                Enemy _enemyScript = _enemyObject.GetComponent<Enemy>();
                _enemyScript.Initialization(this, _gameManager, _dataWaysPoints);
                _enemyScript.SetWayPoints(_currentWay);
                //_enemyScript.SetLayer(_startLayerEnemy);
                //_startLayerEnemy++;
                _startLayerEnemy--;

                _timeWaitForNextSpawnEnemy = Random.Range(_minTimeWaitForNextSpawnEnemy, _maxTimeWaitForNextSpawnEnemy);
                yield return new WaitForSeconds(_timeWaitForNextSpawnEnemy);
            }
        }

        _enemyAmountInWave++;
    }

    private void ChooseEnemyWay() {
        if (_dataWaysPoints.Count == 1) {
            _currentWay = _dataWaysPoints[0];
        }
        else {
            int randomWayPoints = Random.Range(0, _dataWaysPoints.Count);
            //print("random number = " + randomWayPoints);
            _currentWay = _dataWaysPoints[randomWayPoints];
        }
    }

    private void SetPointSpawn() {
        _pointSpawn = _currentWay.WayPoints[0].position;
    }

    private void OnDestroy() {
        Enemy.EnemySpawnerTest -= RemoveEnemy2;
    }
}
