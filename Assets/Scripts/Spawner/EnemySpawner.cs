using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField]
    private int _amountEnemyInWawe;
    private List<Wave> _waves = new List<Wave>();

    [Header("Parametrs")]
    [SerializeField]
    private Wave _wave;
    [SerializeField]
    private List<WaveData> _wavesData = new List<WaveData>();
    [SerializeField]
    private bool _isNpc;

    public int countWave = 0;
    public NPCMinotaur npc;
    public GameObject waveObject;
    public GameManager gameManager;
    public Camera mainCamera;

    public List<WaveData> WavesData { get => _wavesData; }
    public bool IsNpc { get => _isNpc; }
    public int Waves { get => _waves.Count; }
    public bool IsTheLastEnemyInTheLastWave {
        get {
            if (countWave == _wavesData.Count && IsTheLastEnemyInCurrentWave) {
                return true;
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
        countWave = 0;
        SpawnWaves();
        CalculationEnemyInCurrentWave();
    }

    private void Start() {
        EnableTimerWave();
    }

    private void SpawnWaves() {
        for (int i = 0; i < _wavesData.Count; i++) {
            Wave wave = Instantiate(_wave);
            wave.Init(_wavesData[i], gameManager, mainCamera, this);
            wave.transform.SetParent(waveObject.transform);
            _waves.Add(wave);
        }
    }

    public void CalculationEnemyInCurrentWave() {
        for (int numberSpawn = 0; numberSpawn < _waves[countWave].Spawns.Count; numberSpawn++) {
            _amountEnemyInWawe += _waves[countWave].Spawns[numberSpawn].AmountEnemies;
        }
    }

    public void RemoveEnemyInCurrentWave() {
        _amountEnemyInWawe--;
    }

    public void EnableTimerWave() {
        foreach (SpawnEnemyData spawn in _wavesData[countWave].spawnsEnemyData) {
            spawn.startWave.SetCurrentRules();
            spawn.startWave.gameObject.SetActive(true);
        }
    }

    private void DisableTimerWave() {
        foreach (SpawnEnemyData spawn in _wavesData[countWave - 1].spawnsEnemyData) {
            spawn.startWave.gameObject.SetActive(false);
        }
    }

    public void EnableWaveEnemy() {
        _waves[countWave - 1].EnableSpawns();
        gameManager.SetWaveText();
        DisableTimerWave();
    }
}
