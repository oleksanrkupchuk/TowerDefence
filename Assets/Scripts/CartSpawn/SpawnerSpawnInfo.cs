using UnityEngine;

public class SpawnerSpawnInfo : MonoBehaviour {
    [SerializeField]
    private EnemySpawner _enemySpawner;
    [SerializeField]
    private SpawnInfo _spawnInfo;

    private void Awake() {
        SpawnCartInfoAboutSpawn();
    }

    private void SpawnCartInfoAboutSpawn() {
        for (int numberWave = 0; numberWave < _enemySpawner.WavesData.Count; numberWave++) {
            SpawnSpawn(_enemySpawner.WavesData[numberWave]);
        }
    }

    private void SpawnSpawn(WaveData waveData) {
        for (int numberSpawn = 0; numberSpawn < waveData.spawnsEnemyData.Count; numberSpawn++) {
            SpawnSpawn(waveData.spawnsEnemyData[numberSpawn]);
        }
    }

    private void SpawnSpawn(SpawnEnemyData spawnEnemyData) {
        SpawnInfo _spawnInfoObject = Instantiate(_spawnInfo);
        _spawnInfoObject.transform.SetParent(transform);
        _spawnInfoObject.transform.localScale = new Vector3(1f, 1f);
        //_spawnInfoObject.Init(spawnEnemyData);
    }
}
