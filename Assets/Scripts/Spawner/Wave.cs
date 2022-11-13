using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    private List<SpawnEnemy> _spawns = new List<SpawnEnemy>();
    private WaveData _waveData;
    private GameManager _gameManager;
    private Camera _camera;
    private EnemySpawner _enemySpawner;
    private Roads _road;
    public List<SpawnEnemy> Spawns { get => _spawns; }

    public void Init(WaveData waveData, GameManager gameManager, 
        Camera camera, EnemySpawner enemySpawner, Roads road) {
        _waveData = waveData;
        _gameManager = gameManager;
        _enemySpawner = enemySpawner;
        _camera = camera;
        _road = road;

        SpawnEnemySpawns();
    }

    private void SpawnEnemySpawns() {
        for (int i = 0; i < _waveData.spawnsEnemyData.Count; i++) {
            SpawnEnemy _spawnEnemyObject = Instantiate(Resources.Load("SpawnEnemy", typeof(SpawnEnemy))) as SpawnEnemy;
            _spawnEnemyObject.Init(_waveData.spawnsEnemyData[i], _gameManager, _camera, _enemySpawner, _road);
            _spawns.Add(_spawnEnemyObject);
            _spawnEnemyObject.transform.SetParent(gameObject.transform);

            _waveData.spawnsEnemyData[i].startWaveIcon.Init(_enemySpawner);
        }
    }

    public void EnableSpawns() {
        for (int i = 0; i < _spawns.Count; i++) {
            _spawns[i].EnableSpawnsEnemy();
        }
    }
}
