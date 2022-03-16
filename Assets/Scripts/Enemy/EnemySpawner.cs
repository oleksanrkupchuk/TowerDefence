using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField]
    private int _amountEnemyInWawe;
    [SerializeField]
    private int _countEnemies;
    private int _counttWave = 0;
    private Wave _currentWave;

    [Header("Game Manager")]
    [SerializeField]
    private GameManager _gameManager;

    [Header("Parametrs")]
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private List<Wave> _waves = new List<Wave>();
    [SerializeField]
    private List<Enemy> _enemies = new List<Enemy>();

    public bool IsTheLastEnemyInTheLastWave {
        get {
            if (_currentWave == _waves[_waves.Count - 1] && IsTheLastEnemyInCurrentWave) {
                return true;
            }
            else if (IsTheLastEnemyInCurrentWave) {
                _counttWave++;
                _currentWave = _waves[_counttWave];
            }

            return false;
        }
    }

    public bool IsTheLastEnemyInCurrentWave {
        get {
            if (_amountEnemyInWawe == 0) {
                return true;
            }

            return false;
        }
    }

    private void OnEnable() {
        _currentWave = _waves[0];
        SpawnWaves();
    }

    private void SpawnWaves() {
        for (int numberWave = 0; numberWave < _waves.Count; numberWave++) {
            SpawnSpawns(_waves[numberWave]);
        }
    }

    private void SpawnSpawns(Wave wave) {
        for (int numberSpawn = 0; numberSpawn < wave.spawns.Count; numberSpawn++) {
            SpawnChains(wave.spawns[numberSpawn]);
        }
    }

    private void SpawnChains(Spawn spawn) {
        for (int numberChain = 0; numberChain < spawn.chains.Count; numberChain++) {
            SpawnChainEnemies(spawn.chains[numberChain], spawn);
        }
    }

    private void SpawnChainEnemies(Chain chain, Spawn spawn) {
        for (int numberEnemyInList = 0; numberEnemyInList < chain.chainListEnemies.Count; numberEnemyInList++) {
            EnemySpawnRules _enemySpawnRules = chain.chainListEnemies[numberEnemyInList];
            SpawnEnemies(_enemySpawnRules, spawn);
        }
    }

    private void SpawnEnemies(EnemySpawnRules enemySpawnRules, Spawn spawn) {
        for (int countEnemy = 0; countEnemy < enemySpawnRules.amount; countEnemy++) {
            Enemy _enemy = Instantiate(enemySpawnRules.enemy, spawn.spawnPoint.position, Quaternion.identity);
            _enemy.name = "enemy " + countEnemy;
            _enemy.Init(_gameManager, this, _camera);
            _enemy.SetWayPoints(spawn.wayPoints);

            if (enemySpawnRules.needUnlockEnemy) {
                _enemy.gameObject.AddComponent<UnlockEnemy>();
                enemySpawnRules.needUnlockEnemy = false;
            }
            _enemy.gameObject.SetActive(false);
            _enemies.Add(_enemy);
        }
    }

    private void AddEnemyInCurrentWave() {
        _amountEnemyInWawe++;
    }

    public void RemoveEnemyInCurrentWave() {
        _amountEnemyInWawe--;
    }

    public void EnableTimerWave() {
        foreach (Spawn spawn in _currentWave.spawns) {
            spawn.timerWave.gameObject.SetActive(true);
        }
    }

    public void EnableWaveEnemy() {
        _gameManager.UpdateWaveText(_counttWave, _waves.Count);

        for (int i = 0; i < _currentWave.spawns.Count; i++) {
            EnableSpawnsEnemy(_currentWave.spawns[i]);
        }
    }

    private void EnableSpawnsEnemy(Spawn spawn) {
        for (int i = 0; i < spawn.chains.Count; i++) {
            StartCoroutine(EnableChainOfEnemies(spawn.chains[i]));
        }
    }

    private IEnumerator EnableChainOfEnemies(Chain chaine) {
        for (int i = 0; i < chaine.chainListEnemies.Count; i++) {
            yield return StartCoroutine(EnableEnemies(chaine.chainListEnemies[i]));
        }
    }

    private IEnumerator EnableEnemies(EnemySpawnRules enemySpawnRules) {
        for (int i = 0; i < enemySpawnRules.amount; i++) {
            _enemies[_countEnemies].gameObject.SetActive(true);
            _countEnemies++;
            AddEnemyInCurrentWave();

            float _timeWaitForNextSpawnEnemy = Random.Range(enemySpawnRules.minTimeDelayForNextEnemy, enemySpawnRules.maxTimeDelayForNextEnemy);
            yield return new WaitForSeconds(_timeWaitForNextSpawnEnemy);
        }
    }
}
