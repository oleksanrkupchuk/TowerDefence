using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaveData {
    public List<SpawnEnemyData> spawnsEnemyData = new List<SpawnEnemyData>();
}

[Serializable]
public class SpawnEnemyData {
    public int roadName;
    public StartWaveIcon startWaveIcon;
    public List<ChainEnemyData> spawnEnemies = new List<ChainEnemyData>();
}

[Serializable]
public class ChainEnemyData {
    public RoadPart roadPart = new RoadPart();
    public List<PointEnemyData> pointEnemyData = new List<PointEnemyData>();
}