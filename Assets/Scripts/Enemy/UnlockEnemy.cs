using UnityEngine;

public class UnlockEnemy : MonoBehaviour {
    private EnemyCartData _enemyCartData;
    private float _leftBorderCamera = -14.21f;
    private float _bottomBorderCamera = -6.2f;

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
            }
        }
    }
}
