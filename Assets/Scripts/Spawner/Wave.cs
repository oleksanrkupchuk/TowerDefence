using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    private List<Spawn> _spawns = new List<Spawn>();
    private WaveData _waveData;
    private GameManager _gameManager;
    private Camera _camera;
    private EnemySpawner _enemySpawner;
    public List<Spawn> Spawns { get => _spawns; }

    public void Init(WaveData waveData, GameManager gameManager, Camera camera, EnemySpawner enemySpawner) {
        _waveData = waveData;
        _gameManager = gameManager;
        _enemySpawner = enemySpawner;
        _camera = camera;

        SpawnEnemySpawns();
    }

    private void SpawnEnemySpawns() {
        for (int i = 0; i < _waveData.spawnsEnemyData.Count; i++) {
            Spawn _spawnObject = Instantiate(Resources.Load("Spawn", typeof(Spawn))) as Spawn;
            _spawnObject.Init(_waveData.spawnsEnemyData[i], _gameManager, _camera, _enemySpawner);
            _spawns.Add(_spawnObject);
            _spawnObject.transform.SetParent(gameObject.transform);

            _waveData.spawnsEnemyData[i].startWaveIcon.Init(_enemySpawner);
        }
    }

    public void EnableSpawns() {
        for (int i = 0; i < _spawns.Count; i++) {
            _spawns[i].EnableSpawnsEnemy();
        }
    }
}
