using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Wave {
    public List<Spawn> spawns = new List<Spawn>();
}

[Serializable]
public class Spawn {
    public Transform spawnPoint;
    public TimerWave timerWave;
    public List<Transform> wayPoints = new List<Transform>();
    public List<EnemySpawnRules> listEnemies = new List<EnemySpawnRules>();
    public List<Chain> chains = new List<Chain>();
}

[Serializable]
public class Chain {
    public List<EnemySpawnRules> chainListEnemies = new List<EnemySpawnRules>();
}

[Serializable]
public class EnemySpawnRules {
    [Range(1, 20)]
    public int amount;
    public float minTimeDelayForNextEnemy;
    public float maxTimeDelayForNextEnemy;
    public Enemy enemy;
    public bool needUnlockEnemy;
}