using UnityEngine;

public class SatyrDoctor : Enemy {
    private float _halfHealth;
    private bool _isHealing = false;

    [SerializeField]
    private float _healing;
    [SerializeField]
    private EnemyRange _enemyRange;

    private new void Start() {
        base.Start();
        _halfHealth = _healthMax / 2;
    }

    private new void Update() {
        base.Update();
    }

    public override void TakeDamage(float damage) {
        if (!IsDead) {
            _health -= damage;

            CheckHealthAndHealingEnemy();
            ShiftHealthBar();

            if (IsDead) {
                DeathFromBullet();
            }
        }
    }

    private void CheckHealthAndHealingEnemy() {
        if (_isHealing) {
            return;
        }

        if (_health <= _halfHealth) {
            Healing();
        }
    }

    private void Healing() {
        print("health");
        _isHealing = true;
        AddHealth(_healing);
        PlayHealingEffect();

        foreach (var enemy in _enemyRange.Enemies) {
            enemy.AddHealth(_healing);
            enemy.PlayHealingEffect();
        }
    }
}
