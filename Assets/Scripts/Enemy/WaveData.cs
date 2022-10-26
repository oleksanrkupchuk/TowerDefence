using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaveData {
    public List<SpawnEnemyData> spawnsEnemyData = new List<SpawnEnemyData>();
}

[Serializable]
public class SpawnEnemyData {
    public StartWaveIcon startWaveIcon;
    public List<ChainData> chainsData = new List<ChainData>();
}

[Serializable]
public class ChainData {
    public List<Transform> wayPoints = new List<Transform>();
    public List<ChainOfEnemies> chainListEnemies = new List<ChainOfEnemies>();
    public RoadPart wayPointType = new RoadPart();
}