using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnChainEnemy : MonoBehaviour {
    private SpawnEnemyData _spawnEnemyData;
    private GameManager _gameManager;
    private EnemySpawner _enemySpawner;
    private Roads _roads;
    private Camera _camera;
    private ChainEnemyData _chainEnemy;
    private int _amountEnemyInChain = 0;
    private List<Enemy> enemies = new List<Enemy>();
    public List<Enemy> Enemies { get => enemies; }

    [HideInInspector]
    public List<PointEnemyData> rules = new List<PointEnemyData>();

    public void Init(SpawnEnemyData spawnData, ChainEnemyData chainData, GameManager gameManager, 
        Camera camera, EnemySpawner enemySpawner, Roads road) {
        _spawnEnemyData = spawnData;
        _chainEnemy = chainData;
        _gameManager = gameManager;
        _camera = camera;
        _enemySpawner = enemySpawner;
        _roads = road;

        InstantiateEnemy();
    }

    private void InstantiateEnemy() {
        for (int numberListEnemy = 0; numberListEnemy < _chainEnemy.pointEnemyData.Count; numberListEnemy++) {
            for (int amountEnemy = 0; amountEnemy < _chainEnemy.pointEnemyData[numberListEnemy].amount; amountEnemy++) {
                PointEnemyData _pointEnemyData = _chainEnemy.pointEnemyData[numberListEnemy];
                List<Transform> _roadPart = _roads.GetPartRoad(_spawnEnemyData.roadName, _chainEnemy.roadPart);

                GameObject _enemyObject = Instantiate(_pointEnemyData.enemy, _roadPart[0].position, Quaternion.identity);
                //Enemy _enemy = _enemyObject.GetComponent<Enemy>();
                if(!_enemyObject.TryGetComponent(out Enemy enemy)) {
                    Debug.LogError($"Can`t get component from an {_enemyObject}");
                    return;
                }

                Enemy _enemy = _enemyObject.GetComponent<Enemy>();
                _enemy.name = _enemy.name + " " + amountEnemy;
                _enemy.Init(_gameManager, _enemySpawner, _camera);
                _enemy.SetWayPoints(_roadPart);

                if (_enemySpawner.IsNpc) {
                    _enemy.DisableBetweetCollider(_enemy.AttackCollider, _enemySpawner.npc.AttackCollider);
                }

                if (_pointEnemyData.needUnlockEnemy) {
                    _enemy.gameObject.AddComponent<UnlockEnemy>();
                    _pointEnemyData.needUnlockEnemy = false;
                }
                _enemy.transform.SetParent(gameObject.transform);
                _enemy.gameObject.SetActive(false);
                enemies.Add(_enemy);
            }

            rules.Add(_chainEnemy.pointEnemyData[numberListEnemy]);
        }
    }

    public IEnumerator EnableChainOfEnemies() {
        for (int i = 0; i < _chainEnemy.pointEnemyData.Count; i++) {
            yield return StartCoroutine(EnableEnemies(_chainEnemy.pointEnemyData[i]));
        }

    }

    private IEnumerator EnableEnemies(PointEnemyData enemySpawnRules) {
        yield return new WaitForSeconds(enemySpawnRules._waitTimeUntilToSpawn);

        for (int i = 0; i < enemySpawnRules.amount; i++) {
            enemies[_amountEnemyInChain].gameObject.SetActive(true);
            _amountEnemyInChain++;

            float _timeWaitForNextSpawnEnemy = Random.Range(enemySpawnRules.minTimeDelayForNextEnemy, enemySpawnRules.maxTimeDelayForNextEnemy);
            yield return new WaitForSeconds(_timeWaitForNextSpawnEnemy);
            //yield return new WaitForSeconds(4f);
        }
    }
}