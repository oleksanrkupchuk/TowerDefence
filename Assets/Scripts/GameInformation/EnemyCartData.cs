using UnityEngine;

[CreateAssetMenu(fileName = "CartEnemyData", menuName = "ScriptableObjects/CartEnemyData", order = 2)]
public class EnemyCartData : ScriptableObject
{
    public bool unlockEnemy;
    public Sprite unlockEnemyIcon;
    public Sprite lockEnemyIcon;
    public new string name;
    [TextArea(4, 10)]
    public string description;
    public Enemy enemy;
    public DebuffCartData[] debuffs;
}
