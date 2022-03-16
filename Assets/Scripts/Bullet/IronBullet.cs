using UnityEngine;

public class IronBullet : Bullet {
    public bool thonr;

    private new void Start() {
        base.Start();
    }

    private new void Update() {
        base.Update();
    }

    protected void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.TryGetComponent(out Enemy enemy)) {
            if (_target == enemy) {
                _target.LastPosition -= SetTargetPosition;
                _target.TakeDamage(_damage);
                ChecBuyAbilityAndSlowEnemy(_target);
                SetTargetPositionAndSetTargetNull();
            }
        }
    }

    private void ChecBuyAbilityAndSlowEnemy(Enemy _enemy) {
        if (thonr) {
            _enemy.Debuff.StartSlowMove();
        }
    }
}
