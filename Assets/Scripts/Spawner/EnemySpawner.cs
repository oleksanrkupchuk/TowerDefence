using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    private int _amountEnemyInWawe;

    [SerializeField]
    private Roads _road;
    [SerializeField]
    private List<WaveData> wavesData = new List<WaveData>();
    [SerializeField]
    private bool isNpc;

    [HideInInspector]
    public List<Wave> waves = new List<Wave>();
    [HideInInspector]
    public int countWave = 0;
    public NPCMinotaur npc;
    public GameObject waveObject;
    public GameManager gameManager;
    public Camera mainCamera;

    public List<WaveData> WavesData { get => wavesData; }
    public bool IsNpc { get => isNpc; }
    public int Waves { get => waves.Count; }
    public bool IsTheLastEnemyInTheLastWave {
        get {
            if (countWave == wavesData.Count && IsTheLastEnemyInCurrentWave) {
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
        if (_road == null) {
            Debug.LogError($"Field {_road} is empty");
            return;
        }
        if (gameManager == null) {
            Debug.LogError($"Field {gameManager} is empty");
            return;
        }
        if (mainCamera == null) {
            Debug.LogError($"Field {mainCamera} is empty");
            return;
        }

        countWave = 0;
        SpawnWaves();
        CalculationEnemyInCurrentWave();
    }

    private void Start() {
        EnableTimerWave();
    }

    private void SpawnWaves() {
        for (int i = 0; i < wavesData.Count; i++) {
            Wave wave = Instantiate(Resources.Load(PathPrefab.wavePrefab, typeof(Wave))) as Wave;
            wave.Init(wavesData[i], gameManager, mainCamera, this, _road);
            wave.transform.SetParent(waveObject.transform);
            waves.Add(wave);
        }
    }

    public void CalculationEnemyInCurrentWave() {
        for (int numberSpawn = 0; numberSpawn < waves[countWave].Spawns.Count; numberSpawn++) {
            _amountEnemyInWawe += waves[countWave].Spawns[numberSpawn].AmountEnemies;
        }
    }

    public void RemoveEnemyInCurrentWave() {
        _amountEnemyInWawe--;
    }

    public void EnableTimerWave() {
        foreach (SpawnEnemyData spawn in wavesData[countWave].spawnsEnemyData) {
            spawn.startWaveIcon.SetCurrentRules();
            spawn.startWaveIcon.gameObject.SetActive(true);
        }
    }

    private void DisableTimerWave() {
        foreach (SpawnEnemyData spawn in wavesData[countWave - 1].spawnsEnemyData) {
            spawn.startWaveIcon.gameObject.SetActive(false);
        }
    }

    public void EnableWaveEnemy() {
        waves[countWave - 1].EnableSpawns();
        gameManager.SetWaveText();
        DisableTimerWave();
    }
}
