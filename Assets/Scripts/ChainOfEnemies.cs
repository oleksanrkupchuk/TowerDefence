using UnityEngine;

[CreateAssetMenu(fileName = "ChainOfEnemies", menuName = "ScriptableObjects/ChainOfEnemies", order = 3)]
public class ChainOfEnemies : ScriptableObject
{
    [Range(1, 50)]
    public int amount;
    public float _waitTimeUntilToSpawn;
    public Enemy enemy;
    public float minTimeDelayForNextEnemy;
    public float maxTimeDelayForNextEnemy;
    public bool needUnlockEnemy;
}
