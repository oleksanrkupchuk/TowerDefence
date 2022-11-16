using System;
using UnityEngine;

public class UnlockEnemy : MonoBehaviour {
    private EnemyCartData _enemyCartData;
    public static event Action IsUnlockEnemy;

    private void Start() {
        _enemyCartData = GetComponent<Enemy>().EnemyCartData;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag(Tags.areaUnlockEnemy)) {
            _enemyCartData.isUnlockEnemy = true;
            IsUnlockEnemy();
        }
    }
}
