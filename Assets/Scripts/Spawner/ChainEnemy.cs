using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainEnemy : MonoBehaviour
{
    private GameManager _gameManager;
    private EnemySpawner _enemySpawner;
    private Camera _camera;
    private ChainData _chainData;
    private SpawnEnemyData _spawnEnemyData;
    private int _amountEnemyInChain = 0;
    public List<Enemy> enemies = new List<Enemy>();
    public int CountEnemies { get => enemies.Count; }

    public void Init(ChainData chainData, SpawnEnemyData spawnEnemyData, GameManager gameManager, Camera camera, EnemySpawner enemySpawner) {
        _chainData = chainData;
        _spawnEnemyData = spawnEnemyData;
        _gameManager = gameManager;
        _camera = camera;
        _enemySpawner = enemySpawner;

        InstantiateEnemy();
    }

    private void InstantiateEnemy() {
        for (int numberListEnemy = 0; numberListEnemy < _chainData.chainListEnemies.Count; numberListEnemy++) {
            for (int amountEnemy = 0; amountEnemy < _chainData.chainListEnemies[numberListEnemy].amount; amountEnemy++) {
                EnemySpawnRules _enemySpawnRules = _chainData.chainListEnemies[numberListEnemy];
                Enemy _enemy = Instantiate(_enemySpawnRules.enemy,
                    _spawnEnemyData.spawnPoint.position, Quaternion.identity);

                _enemy.name = _enemy.name + " " + amountEnemy;
                _enemy.Init(_gameManager, _enemySpawner, _camera);
                _enemy.SetWayPoints(_spawnEnemyData.wayPoints);

                if (_enemySpawnRules.needUnlockEnemy) {
                    _enemy.gameObject.AddComponent<UnlockEnemy>();
                    _enemySpawnRules.needUnlockEnemy = false;
                }
                _enemy.transform.SetParent(gameObject.transform);
                _enemy.gameObject.SetActive(false);
                enemies.Add(_enemy);
            }
        }
    }

    public IEnumerator EnableChainOfEnemies() {
        for (int i = 0; i < _chainData.chainListEnemies.Count; i++) {
            yield return StartCoroutine(EnableEnemies(_chainData.chainListEnemies[i]));
        }

    }

    private IEnumerator EnableEnemies(EnemySpawnRules enemySpawnRules) {
        yield return new WaitForSeconds(enemySpawnRules.timeSpawnForNextChain);

        for (int i = 0; i < enemySpawnRules.amount; i++) {
            enemies[_amountEnemyInChain].gameObject.SetActive(true);
            _amountEnemyInChain++;


            float _timeWaitForNextSpawnEnemy = Random.Range(enemySpawnRules.minTimeDelayForNextEnemy, enemySpawnRules.maxTimeDelayForNextEnemy);
            yield return new WaitForSeconds(_timeWaitForNextSpawnEnemy);
        }
    }
}
