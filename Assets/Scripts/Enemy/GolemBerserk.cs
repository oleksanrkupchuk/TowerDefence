public class GolemBerserk : Enemy
{
    private float _stepHealth;
    private float _fallHealth;

    private new void Start()
    {
        base.Start();
        _stepHealth = _healthMax / 7;
        _fallHealth = _healthMax;
    }

    private new void Update()
    {
        base.Update();
    }

    public override void TakeDamage(float damage) {
        if (!IsDead) {
            _health -= damage;
            IncreaseSpeedWhenFallHealth();
            SoundManager.Instance.PlaySoundEffect(SoundName.HitEnemy);
            ShiftHealthBar();

            if (IsDead) {
                DeathFromBullet();
            }

        }
    }

    private void IncreaseSpeedWhenFallHealth() {
        float _differentHealth = _fallHealth - _health;
        int _coeficient = (int)(_differentHealth / _stepHealth);
        _speed += 0.15f * _coeficient;
        SetSpeedAnimationWalking(_speed);
        _fallHealth -= _coeficient * _stepHealth;
    }
}
