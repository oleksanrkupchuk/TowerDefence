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
    public List<Transform> wayPoints = new List<Transform>();
    public List<EnemySpawn> enemies = new List<EnemySpawn>();
}

[Serializable]
public class EnemySpawn {
    public Enemy enemy;
    [Range(1, 20)]
    public int amount;
}