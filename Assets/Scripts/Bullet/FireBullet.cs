using UnityEngine;

public class FireBullet : Bullet {
    private bool _isEndWay = false;

    [SerializeField]
    private FireArea _fireArea;

    [HideInInspector]
    public int chanceFireArea;
    public bool burning;
    public bool fireArea;

    private new void Start() {
        base.Start();
    }

    private new void Update() {
        base.Update();
    }

    protected override void CalculationT() {
        _timeWay += Time.deltaTime;
        _t = _timeWay / _timeFlight;
    }

    protected override void CheckTAndDestroyBullet() {
        if (!_isEndWay) {
            if (_t >= 1) {
                CheckBuyFireAreaAbilityAndBurnEnemy();
                PlayAnimationDestroy();
                _isEndWay = true;
            }
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.TryGetComponent(out Enemy enemy)) {
            if (enemy == _target) {
                _tower.RemoveBullet(this);
                _target.LastPosition -= SetTargetPosition;
                _target.TakeDamage(_damage);
                CheckBuyBurningAbilityAndBurnEnemy(_target);
                //_circleCollider.enabled = false;
                SetTargetPositionAndSetTargetNull();
            }
        }
    }

    private void CheckBuyBurningAbilityAndBurnEnemy(Enemy _enemy) {
        if (burning) {
            _enemy.Debuff.StartBurning();
        }
    }

    private void CheckBuyFireAreaAbilityAndBurnEnemy() {
        if (fireArea) {
            int _chance = Random.Range(0, 100);
            if (_chance <= chanceFireArea) {
                Instantiate(_fireArea.gameObject, _targetPosition, Quaternion.identity);
            }
        }
    }
}
