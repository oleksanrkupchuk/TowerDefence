using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField]
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
    private GameObject _waveObject;
    [SerializeField]
    private Wave _wave;
    [SerializeField]
    private List<WaveData> _wavesData = new List<WaveData>();

    public List<WaveData> WavesData { get => _wavesData; }


    public int Waves { get => _waves.Count; }
    public int CountWave { get => _counttWave; }

    public bool IsTheLastEnemyInTheLastWave {
        get {
            if (_currentWave == _wavesData[_wavesData.Count - 1] && IsTheLastEnemyInCurrentWave) {
                return true;
            }

            return false;
        }
    }

    public bool IsTheLastEnemyInCurrentWave {
        get {
            if (_amountEnemyInWawe == 0) {
                if (_counttWave + 1 < _wavesData.Count) {
                    _counttWave++;
                    _currentWave = _wavesData[_counttWave - 1];
                    CalculationEnemyInCurrentWave();
                }
                return true;
            }

            return false;
        }
    }

    private void OnEnable() {
        _currentWave = _wavesData[0];
        SpawnWaves();
        CalculationEnemyInCurrentWave();
        EnableTimerWave();
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
            _amountEnemyInWawe += _waves[_counttWave].Spawns[numberSpawn].AmountEnemies;
        }
    }

    public void RemoveEnemyInCurrentWave() {
        _amountEnemyInWawe--;
    }

    public void EnableTimerWave() {
        foreach (SpawnEnemyData spawn in _currentWave.spawnsEnemyData) {
            spawn.newWave.SetCurrentRules();
            spawn.newWave.gameObject.SetActive(true);
        }
    }

    private void DisableTimerWave() {
        foreach (SpawnEnemyData spawn in _currentWave.spawnsEnemyData) {
            spawn.newWave.gameObject.SetActive(false);
        }
    }

    public void EnableWaveEnemy() {
        _waves[_counttWave].EnableSpawns();
        _gameManager.SetWaveText(CountWave + 1);
        DisableTimerWave();
    }
}
