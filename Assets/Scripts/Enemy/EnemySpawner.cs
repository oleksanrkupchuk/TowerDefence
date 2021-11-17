using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private static EnemySpawner _enemySpawner = null;
    public static EnemySpawner Instance { get => _enemySpawner; }

    [SerializeField] 
    private GameObject enemyPrefab;
    [SerializeField] 
    private int _enemyAmountInWave;
    [SerializeField] 
    private int _enemyAmountSpawn;
    [SerializeField] 
    private int _quantityWave;
    private int _wave = 0;

    [SerializeField]
    private List<DataWayPoints> _dataWayPoints = new List<DataWayPoints>();

    [SerializeField]
    private EnemyList _listEnemys;

    //[SerializeField]
    //private MenuLevels _menuLevels;
    [SerializeField]
    private float _minTimeWaitForNextSpawnEnemy;
    [SerializeField]
    private float _maxTimeWaitForNextSpawnEnemy;
    private float _timeWaitForNextSpawnEnemy;

    public bool IsLastWave {
        get {
            if(_wave >= _quantityWave) {
                return true;
            }

            return false;
        }
    }

    private void Awake() {
        if(_enemySpawner == null) {
            _enemySpawner = this;
        }
        else if(_enemySpawner != this) {
            Destroy(gameObject);
        }

        //StartSpawn();
    }

    private void Start()
    {
        //StartSpawn();
    }

    public void StartSpawn() {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn() {
        if(_wave < _quantityWave) {
            for (int i = 0; i < _enemyAmountInWave; i++) {
                enemyPrefab.name = "enemy " + i;
                GameObject _enemyObject = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
                Enemy _enemyScript = _enemyObject.GetComponent<Enemy>();
                _enemyScript.Initialization(_listEnemys, _dataWayPoints);

                _timeWaitForNextSpawnEnemy = Random.Range(_minTimeWaitForNextSpawnEnemy, _maxTimeWaitForNextSpawnEnemy);
                yield return new WaitForSeconds(_timeWaitForNextSpawnEnemy);
            }
            _wave++;
        }

        _enemyAmountInWave++;
    }

    public void ResetEnemyQuantityInSpawn() {
        _enemyAmountSpawn = 0;
    }
}
