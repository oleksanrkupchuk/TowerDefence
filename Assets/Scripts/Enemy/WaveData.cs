using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaveData {
    public List<SpawnEnemyData> spawnsEnemyData = new List<SpawnEnemyData>();
}

[Serializable]
public class SpawnEnemyData {
    public Transform spawnPoint;
    public NewWaveIcon newWaveIcon;
    public Sprite iconTimerWave;
    public List<Transform> wayPoints = new List<Transform>();
    public List<ChainData> chainsData = new List<ChainData>();
}

[Serializable]
public class ChainData {
    public List<EnemySpawnRules> chainListEnemies = new List<EnemySpawnRules>();
}

[Serializable]
public class EnemySpawnRules {
    [Range(1, 20)]
    public int amount;
    public float timeSpawnForNextChain;
    public Enemy enemy;
    public float minTimeDelayForNextEnemy;
    public float maxTimeDelayForNextEnemy;
    public bool needUnlockEnemy;
}