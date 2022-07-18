using UnityEngine;

public class SatyrRunner : Enemy {
    private float _timer;

    [SerializeField]
    private float _minTimeIncreaseEnemySpeed;
    [SerializeField]
    private float _maxTimeIncreaseEnemySpeed;
    [SerializeField]
    private EnemyRange _enemyRange;
    [SerializeField]
    private int _percentageOfAdditionalSpeed;

    private new void Start() {
        _timer = Random.Range(_minTimeIncreaseEnemySpeed, _maxTimeIncreaseEnemySpeed);
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
            _timer = _minTimeIncreaseEnemySpeed;
        }
    }

    private void StartIncreaseSpeed() {
        _timer = Random.Range(_minTimeIncreaseEnemySpeed, _maxTimeIncreaseEnemySpeed);

        if (_isIncreaseSpeed) {
            return;
        }

        //print("runner");
        foreach (var enemy in _enemyRange.Enemies) {
            float _speed = enemy.Speed + (enemy.Speed * _percentageOfAdditionalSpeed);
            enemy.SetSpeed(_speed);
            enemy.SetSpeedAnimationWalking(_speed);
            StartCoroutine(enemy.IncreaseSpeed());
        }
    }
}
