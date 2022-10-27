using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaveData {
    public List<SpawnEnemyData> spawnsEnemyData = new List<SpawnEnemyData>();
}

[Serializable]
public class SpawnEnemyData {
    public string roadName;
    public StartWaveIcon startWaveIcon;
    public List<ChainData> chainsData = new List<ChainData>();
}

[Serializable]
public class ChainData {
    public List<ChainOfEnemies> chainListEnemies = new List<ChainOfEnemies>();
    public RoadPart roadPart = new RoadPart();
}