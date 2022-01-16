using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave {
    public List<Spawn> spawn = new List<Spawn>();
}

[System.Serializable]
public class Spawn {
    public Transform spawnPoint;
    public GameObject enemy;
    public int amountEnemy;
    public List<Transform> wayPoints = new List<Transform>();
}
