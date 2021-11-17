using System.Collections.Generic;
using UnityEngine;

public class EnemyList : MonoBehaviour
{
    [SerializeField]
    private EnemySpawner _enemySpawner;
    [SerializeField]
    private MenuLevels _menuLevels;
    [SerializeField]
    private List<Enemy> _enemys = new List<Enemy>();
    public List<Enemy> Enemys { get => _enemys; }

    public void AddEnemy(Enemy enemy) {
        _enemys.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy) {
        _enemys.Remove(enemy);
    }

    public void CheckTheNumberOfEnemiesToZero() {
        if(Enemys.Count <= 0 && !_enemySpawner.IsLastWave) {
            _enemySpawner.ResetEnemyQuantityInSpawn();
            _menuLevels.counter = 5f;
        }
    }
}
