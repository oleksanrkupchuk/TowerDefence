using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    private List<ChainOfEnemies> _rules = new List<ChainOfEnemies>();
    private int _amountEnemies;
    private GameManager _gameManager;
    private EnemySpawner _enemySpawner;
    private Camera _camera;
    private SpawnEnemyData _spawnData;
    private List<ChainEnemy> _chainsEnemy = new List<ChainEnemy>();
    public int AmountEnemies { get => _amountEnemies; }

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
            _chainEnemyObject.Init(_spawnData.chainsData[i], _gameManager, _camera, _enemySpawner);
            _chainsEnemy.Add(_chainEnemyObject);
            _chainEnemyObject.transform.SetParent(gameObject.transform);
            _amountEnemies += _chainEnemyObject.Enemies.Count;
            _rules.AddRange(_chainEnemyObject.rules);
        }

        _spawnData.startWaveIcon.SetListEnemy(_rules);
    }

    public void EnableSpawnsEnemy() {
        for (int numberChain = 0; numberChain < _chainsEnemy.Count; numberChain++) {
            StartCoroutine(_chainsEnemy[numberChain].EnableChainOfEnemies());
        }
    }
}
