using UnityEngine;

[CreateAssetMenu(fileName = "CartEnemyData", menuName = "ScriptableObjects/CartEnemyData", order = 2)]
public class EnemyCartData : ScriptableObject
{
    public bool isUnlockEnemy;
    public Sprite unlockEnemyIcon;
    public Sprite lockEnemyIcon;
    public Enemy enemy;
    public DebuffCartData[] debuffs;
}
