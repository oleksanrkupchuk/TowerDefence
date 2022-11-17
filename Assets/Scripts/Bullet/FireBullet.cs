using UnityEngine;

public class FireBullet : Bullet {

    private FireArea _currentFireArea;

    [Header("Fire Bullet")]
    [SerializeField]
    private FireArea _fireArea;
    [SerializeField]
    private int chanceFireArea;

    public bool burning;
    public bool fireArea;

    private new void OnEnable() {
        base.OnEnable();
    }

    private new void Start() {
        base.Start();
    }

    private new void Update() {
        base.Update();
    }

    protected override void SpecialAction() {
        _isApplySpecialAbility = true;
        CheckBuyFireAreaAbilityAndBurnEnemy();
    }

    private void CheckBuyFireAreaAbilityAndBurnEnemy() {
        if (!fireArea) {
            return;
        }

        int _chance = Random.Range(0, 100);
        if (_chance <= chanceFireArea) {
            SetFireArea();
            _currentFireArea.transform.position = new Vector2(_targetPosition.x, _targetPosition.y);
            _currentFireArea.SetDefaultParam();
        }
    }

    private void SetFireArea() {
        for (int i = 0; i < _bulletAbilities.Count; i++) {
            if(_bulletAbilities[i].gameObject.activeSelf == false) {
                _currentFireArea = (FireArea)_bulletAbilities[i];
                return;
            }
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.TryGetComponent(out Enemy enemy)) {
            if (enemy == _target) {
                _hitEnemy.Play();
                CollisionTarget(enemy);
                //enemy.LastPosition -= SetTargetPosition;
                enemy.Debuff.TakeDamage(_damage);
                CheckBuyBurningAbilityAndBurnEnemy(enemy);
                //_circleCollider.enabled = false;
            }
        }
    }

    private void CheckBuyBurningAbilityAndBurnEnemy(Enemy _enemy) {
        if (burning) {
            _enemy.Debuff.StartBurning();
        }
    }
}
