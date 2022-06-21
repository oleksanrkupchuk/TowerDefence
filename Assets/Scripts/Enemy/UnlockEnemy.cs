using System;
using UnityEngine;

public class UnlockEnemy : MonoBehaviour {
    private EnemyCartData _enemyCartData;
    private float _leftBorderCamera = -26.5f;
    private float _bottomBorderCamera = -14.8f;
    public static event Action IsUnlockEnemy;

    private void Start() {
        _enemyCartData = GetComponent<Enemy>().EnemyCartData;
    }

    private void Update() {
        CheckCrossCameraBorderAndUnlockEnemy();
    }

    private void CheckCrossCameraBorderAndUnlockEnemy() {
        if (!_enemyCartData.unlockEnemy) {
            if (transform.position.x >= _leftBorderCamera && transform.position.y >= _bottomBorderCamera) {
                _enemyCartData.unlockEnemy = true;
                IsUnlockEnemy();
            }
        }
    }
}
