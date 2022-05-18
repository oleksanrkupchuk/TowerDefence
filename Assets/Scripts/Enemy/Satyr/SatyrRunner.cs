using System.Collections;
using UnityEngine;

public class SatyrRunner : Enemy {
    private float _timer;

    [SerializeField]
    private float _timeIncreaseEnemySpeed;
    [SerializeField]
    private float _additionalSpeed;
    [SerializeField]
    private EnemyRange _enemyRange;

    private new void Start() {
        _timer = _timeIncreaseEnemySpeed;
        base.Start();
    }

    private new void Update() {
        base.Update();
        CheckTimerAndSetNewSpeed();
    }

    private void CheckTimerAndSetNewSpeed() {
        _timer -= Time.deltaTime;
        if (_timer <= 0) {
            StartIncreaseSpeed();
            _timer = _timeIncreaseEnemySpeed;
        }
    }

    private void StartIncreaseSpeed() {
        if (_isIncreaseSpeed) {
            return;
        }

        //print("runner");
        foreach (var enemy in _enemyRange.Enemies) {
            float _speed = enemy.Speed + _additionalSpeed;
            enemy.SetSpeed(_speed);
            enemy.SetSpeedAnimationWalking(_speed);
            StartCoroutine(enemy.IncreaseSpeed());
        }
    }
}
