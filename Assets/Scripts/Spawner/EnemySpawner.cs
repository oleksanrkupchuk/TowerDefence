using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    private int _amountEnemyInWawe;
    private int _counttWave = 0;
    private WaveData _currentWave;
    private List<Wave> _waves = new List<Wave>();

    [Header("Game Manager")]
    [SerializeField]
    private GameManager _gameManager;

    [Header("Parametrs")]
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private List<WaveData> _wavesData = new List<WaveData>();

    [SerializeField]
    private GameObject _waveObject;
    [SerializeField]
    private Wave _wave;

    public int Waves { get => _waves.Count; }
    public int CountWave { get => _counttWave; }

    public bool IsTheLastEnemyInTheLastWave {
        get {
            if (_currentWave == _wavesData[_wavesData.Count - 1] && IsTheLastEnemyInCurrentWave) {
                return true;
            }
            else if (IsTheLastEnemyInCurrentWave) {
                _counttWave++;
                _currentWave = _wavesData[_counttWave];
                CalculationEnemyInCurrentWave();
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
        _currentWave = _wavesData[0];
        SpawnWaves();
        CalculationEnemyInCurrentWave();
    }

    private void SpawnWaves() {
        for (int i = 0; i < _wavesData.Count; i++) {
            Wave wave = Instantiate(_wave);
            wave.Init(_wavesData[i], _gameManager, _camera, this);
            wave.transform.SetParent(_waveObject.transform);
            _waves.Add(wave);
        }
    }

    private void CalculationEnemyInCurrentWave() {
        for (int numberSpawn = 0; numberSpawn < _waves[_counttWave].Spawns.Count; numberSpawn++) {
            _amountEnemyInWawe += _waves[_counttWave].Spawns[numberSpawn].CountEnemies;
        }
    }

    public void RemoveEnemyInCurrentWave() {
        _amountEnemyInWawe--;
    }

    public void EnableNewWaveIcon() {
        foreach (SpawnEnemyData spawn in _currentWave.spawnsEnemyData) {
            spawn.newWaveIcon.gameObject.SetActive(true);
        }
    }

    private void DisableTimerWave() {
        foreach (SpawnEnemyData spawn in _currentWave.spawnsEnemyData) {
            spawn.newWaveIcon.gameObject.SetActive(false);
        }
    }

    public void EnableWaveEnemy() {
        _waves[_counttWave].EnableSpawns();
        _gameManager.SetWaveText(CountWave + 1);
        DisableTimerWave();
    }
}
