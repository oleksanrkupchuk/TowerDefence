using UnityEngine;

public class IronBullet : Bullet {
    public bool thonr;
    public bool poison;
    private Thorn _currentThorn;

    private new void Start() {
        base.Start();
    }

    private new void Update() {
        base.Update();
    }

    protected override void SpecialAction() {
        _isApplySpecialAbility = true;
        EnableThorn();
    }

    private void EnableThorn() {
        if (thonr) {
            GetThorn();
            _currentThorn.transform.position = new Vector3(_targetPosition.x, _targetPosition.y);
            _currentThorn.SetParametrsToDefault();
        }
    }

    private void GetThorn() {
        for (int i = 0; i < _bulletAbilities.Count; i++) {
            if (_bulletAbilities[i].gameObject.activeSelf == false) {
                _currentThorn = (Thorn)_bulletAbilities[i];
            }
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.TryGetComponent(out Enemy enemy)) {
            if (_target == enemy) {
                _target.Debuff.TakeDamage(_damage);
                ChecBuyAbilityAndPoisonEnemy(_target);
                CollisionTarget(enemy);
            }
        }
    }

    private void ChecBuyAbilityAndPoisonEnemy(Enemy _enemy) {
        if (poison) {
            _enemy.Debuff.StartPoison();
        }
    }
}
