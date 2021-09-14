using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListEnemys : MonoBehaviour
{
    [SerializeField]
    private List<Enemy> _enemys = new List<Enemy>();
    public List<Enemy> ListEnemy { get => _enemys; }

    public void AddEnemy(Enemy enemy) {
        _enemys.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy) {
        _enemys.Remove(enemy);
    }
}
