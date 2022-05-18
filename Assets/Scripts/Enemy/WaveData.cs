using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaveData {
    public List<SpawnEnemyData> spawnsEnemyData = new List<SpawnEnemyData>();
}

[Serializable]
public class SpawnEnemyData {
    public NewWaveIcon newWave;
    public List<ChainData> chainsData = new List<ChainData>();
}

[Serializable]
public class ChainData {
    public List<Transform> wayPoints = new List<Transform>();
    public List<EnemySpawnRules> chainListEnemies = new List<EnemySpawnRules>();
}

[Serializable]
public class EnemySpawnRules {
    [Range(0, 20)]
    public int amount;
    public float _waitTimeForNexEnemies;
    public Enemy enemy;
    public float minTimeDelayForNextEnemy;
    public float maxTimeDelayForNextEnemy;
    public bool needUnlockEnemy;
}