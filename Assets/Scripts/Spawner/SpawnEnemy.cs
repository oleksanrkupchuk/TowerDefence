using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    private List<PointEnemyData> _rules = new List<PointEnemyData>();
    private int _amountEnemies;
    private GameManager _gameManager;
    private EnemySpawner _enemySpawner;
    private Roads _roads;
    private Camera _camera;
    private SpawnEnemyData _spawnEnemyData;
    private List<SpawnChainEnemy> _chainsEnemy = new List<SpawnChainEnemy>();
    public int AmountEnemies { get => _amountEnemies; }

    public void Init(SpawnEnemyData spawnEnemyData, GameManager gameManager, 
        Camera camera, EnemySpawner enemySpawner, Roads roads) {
        _spawnEnemyData = spawnEnemyData;
        _gameManager = gameManager;
        _enemySpawner = enemySpawner;
        _camera = camera;
        _roads = roads;

        SpawnChainEnemy();
    }

    private void SpawnChainEnemy() {
        for (int i = 0; i < _spawnEnemyData.spawnEnemies.Count; i++) {
            SpawnChainEnemy _SpawnChainEnemyObject = Instantiate(Resources.Load("SpawnChainEnemy", typeof(SpawnChainEnemy))) as SpawnChainEnemy;
            _SpawnChainEnemyObject.Init(_spawnEnemyData, _spawnEnemyData.spawnEnemies[i],
                _gameManager, _camera, _enemySpawner, _roads);
            _chainsEnemy.Add(_SpawnChainEnemyObject);
            _SpawnChainEnemyObject.transform.SetParent(gameObject.transform);
            _amountEnemies += _SpawnChainEnemyObject.Enemies.Count;
            _rules.AddRange(_SpawnChainEnemyObject.rules);
        }

        _spawnEnemyData.startWaveIcon.SetListEnemy(_rules);
    }

    public void EnableSpawnsEnemy() {
        for (int numberChain = 0; numberChain < _chainsEnemy.Count; numberChain++) {
            StartCoroutine(_chainsEnemy[numberChain].EnableChainOfEnemies());
        }
    }
}
