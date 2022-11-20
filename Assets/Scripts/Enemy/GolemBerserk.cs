public class GolemBerserk : Enemy {
    private float _stepHealth;
    private float _fallHealth;
    private float additionalSpeed = 0.15f;

    private new void Start() {
        base.Start();
        _stepHealth = _healthMax / 7;
        _fallHealth = _healthMax;
    }

    private new void Update() {
        base.Update();
    }

    public override void TakeDamage(float damage) {
        if (!IsDead) {
            _health -= damage;
            IncreaseSpeedWhenFallHealth();
            ShiftHealthBar();

            if (IsDead) {
                DeadFromBullet();
            }

        }
    }

    private void IncreaseSpeedWhenFallHealth() {
        float _differentHealth = _fallHealth - _health;
        int _coeficient = (int)(_differentHealth / _stepHealth);
        _speed += additionalSpeed * _coeficient;
        SetSpeedAnimationWalking(_speed);
        _fallHealth -= _coeficient * _stepHealth;
        print("fall health = " + _fallHealth);
    }

    public override void AddHealth(float percentageOfRecovery) {
        float _healthAfterHealing = _health + CalculationAdditionalHealth(percentageOfRecovery);

        _health += percentageOfRecovery;
        if (_health > _healthMax) {
            _health = _healthMax;
        }

        if (_healthAfterHealing > _fallHealth) {
            float _differentHealth = _healthAfterHealing - _fallHealth;
            int _coeficient = (int)(_differentHealth / _stepHealth);
            if (_coeficient == 0) {
                _coeficient = 1;
            }
            if (_fallHealth < _healthMax) {
                _speed -= additionalSpeed * _coeficient;
                SetSpeedAnimationWalking(_speed);
                _fallHealth += _coeficient * _stepHealth;
            }
            print("fall health = " + _fallHealth);
        }

        ShiftHealthBar();
    }
}
