using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    private int _enemies;
    private GameManager _gameManager;
    private EnemySpawner _enemySpawner;
    private Camera _camera;
    private SpawnEnemyData _spawnData;
    private List<ChainEnemy> _chainsEnemy = new List<ChainEnemy>();
    public int CountEnemies { get => _enemies; }

    public void Init(SpawnEnemyData spawnData, GameManager gameManager, Camera camera, EnemySpawner enemySpawner) {
        _spawnData = spawnData;
        _gameManager = gameManager;
        _enemySpawner = enemySpawner;
        _camera = camera;

        SpawnChainEnemy();
    }

    private void SpawnChainEnemy() {
        for (int i = 0; i < _spawnData.chainsData.Count; i++) {
            ChainEnemy _chainEnemyObject = Instantiate(Resources.Load("ChainEnemy", typeof(ChainEnemy))) as ChainEnemy;
            _chainEnemyObject.Init(_spawnData.chainsData[i], _spawnData, _gameManager, _camera, _enemySpawner);
            _chainsEnemy.Add(_chainEnemyObject);
            _chainEnemyObject.transform.SetParent(gameObject.transform);
            _enemies += _chainEnemyObject.CountEnemies;
        }
    }

    public void EnableSpawnsEnemy() {
        for (int numberChain = 0; numberChain < _chainsEnemy.Count; numberChain++) {
            StartCoroutine(_chainsEnemy[numberChain].EnableChainOfEnemies());
        }
    }
}
