using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Transform _pointSpawn;

    [SerializeField] 
    private GameObject enemyPrefab;
    [SerializeField] 
    private int _enemyAmountInWave;
    [SerializeField]
    private int _enemyAmountSpawn;
    [SerializeField] 
    private int _quantityWave;
    private int _currentWave = 0;
    public int CurrentWave { get => _currentWave; }
    [SerializeField]
    private int _startLayerEnemy;
    public int maxLayerEnemy;
    public int minLayerEnemy;

    [SerializeField]
    private List<DataWayPoints> _dataWayPoints = new List<DataWayPoints>();

    [SerializeField]
    private GameManager _gameManager;

    [SerializeField]
    private float _minTimeWaitForNextSpawnEnemy;
    [SerializeField]
    private float _maxTimeWaitForNextSpawnEnemy;
    private float _timeWaitForNextSpawnEnemy;

    [SerializeField]
    private List<Enemy> _enemyList = new List<Enemy>();
    public List<Enemy> EnemyList { get => _enemyList; }

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
            if(EnemyList.Count == 0) {
                return true;
            }

            return false;
        }
    }

    public void StartSpawn() {
        StartCoroutine(EnemySpawn());
    }

    private IEnumerator EnemySpawn() {
        if(_currentWave < _quantityWave) {
            _currentWave++;
            _gameManager.UpdateWaveText(_currentWave);
            for (int i = 0; i < _enemyAmountInWave; i++) {
                enemyPrefab.name = "enemy " + i;
                GameObject _enemyObject = Instantiate(enemyPrefab, _pointSpawn.position, Quaternion.identity);
                Enemy _enemyScript = _enemyObject.GetComponent<Enemy>();
                _enemyScript.Initialization(this, _gameManager, _dataWayPoints);
                _enemyScript.SetLayer(_startLayerEnemy);
                //_startLayerEnemy++;
                _startLayerEnemy--;

                _timeWaitForNextSpawnEnemy = Random.Range(_minTimeWaitForNextSpawnEnemy, _maxTimeWaitForNextSpawnEnemy);
                yield return new WaitForSeconds(_timeWaitForNextSpawnEnemy);
            }
        }

        _enemyAmountInWave++;
    }

    public void AddEnemy(Enemy enemy) {
        _enemyList.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy) {
        _enemyList.Remove(enemy);
    }

    //public bool IsTheLastEnemyInWave() {
    //    if (EnemyList.Count <= 0) {
    //        return true;
    //    }

    //    return false;
    //}

    public void ResetMaxLayer() {
        maxLayerEnemy = 50;
    }

    public void ResetMinLayer() {
        maxLayerEnemy = 1;
    }
}
