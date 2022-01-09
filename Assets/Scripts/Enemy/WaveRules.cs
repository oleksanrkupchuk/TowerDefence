using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveRules {
    public List<SpawnRules> spawnRules = new List<SpawnRules>();
}

[System.Serializable]
public class SpawnRules {
    public Transform spawnPoint;
    public GameObject enemy;
    public int amountEnemy;
    public List<Transform> wayPoints = new List<Transform>();
}
