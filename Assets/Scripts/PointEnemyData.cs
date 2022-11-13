using UnityEngine;

[CreateAssetMenu(fileName = "PointEnemyData", menuName = "ScriptableObjects/PointEnemyData", order = 3)]
public class PointEnemyData : ScriptableObject
{
    [Range(1, 50)]
    public int amount;
    public float _waitTimeUntilToSpawn;
    public GameObject enemy;
    public float minTimeDelayForNextEnemy;
    public float maxTimeDelayForNextEnemy;
    public bool needUnlockEnemy;
}
