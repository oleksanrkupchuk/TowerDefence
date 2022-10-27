using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainEnemy : MonoBehaviour {
    private SpawnEnemyData _spawnData;
    private GameManager _gameManager;
    private EnemySpawner _enemySpawner;
    private Road _road;
    private Camera _camera;
    private ChainData _chainData;
    private int _amountEnemyInChain = 0;
    public List<Enemy> enemies = new List<Enemy>();
    public List<Enemy> Enemies { get => enemies; }

    public List<ChainOfEnemies> rules = new List<ChainOfEnemies>();

    public void Init(SpawnEnemyData spawnData, ChainData chainData, GameManager gameManager, 
        Camera camera, EnemySpawner enemySpawner, Road road) {
        _spawnData = spawnData;
        _chainData = chainData;
        _gameManager = gameManager;
        _camera = camera;
        _enemySpawner = enemySpawner;
        _road = road;

        InstantiateEnemy();
    }

    private void InstantiateEnemy() {
        for (int numberListEnemy = 0; numberListEnemy < _chainData.chainListEnemies.Count; numberListEnemy++) {
            for (int amountEnemy = 0; amountEnemy < _chainData.chainListEnemies[numberListEnemy].amount; amountEnemy++) {
                ChainOfEnemies _enemySpawnRules = _chainData.chainListEnemies[numberListEnemy];
                //Enemy _enemy = Instantiate(_enemySpawnRules.enemy,
                //    _chainData.wayPoints[0].position, Quaternion.identity);
                List<Transform> _roadPart = _road.GetPartRoad(_spawnData.roadName, _chainData.roadPart);

                Enemy _enemy = Instantiate(_enemySpawnRules.enemy, _roadPart[0].position, Quaternion.identity);

                _enemy.name = _enemy.name + " " + amountEnemy;
                _enemy.Init(_gameManager, _enemySpawner, _camera);
                _enemy.SetWayPoints(_roadPart);

                if (_enemySpawner.IsNpc) {
                    _enemy.DisableBetweetCollider(_enemy.AttackCollider, _enemySpawner.npc.AttackCollider);
                }

                if (_enemySpawnRules.needUnlockEnemy) {
                    _enemy.gameObject.AddComponent<UnlockEnemy>();
                    _enemySpawnRules.needUnlockEnemy = false;
                }
                _enemy.transform.SetParent(gameObject.transform);
                _enemy.gameObject.SetActive(false);
                enemies.Add(_enemy);
            }

            rules.Add(_chainData.chainListEnemies[numberListEnemy]);
        }
    }

    public IEnumerator EnableChainOfEnemies() {
        for (int i = 0; i < _chainData.chainListEnemies.Count; i++) {
            yield return StartCoroutine(EnableEnemies(_chainData.chainListEnemies[i]));
        }

    }

    private IEnumerator EnableEnemies(ChainOfEnemies enemySpawnRules) {
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
