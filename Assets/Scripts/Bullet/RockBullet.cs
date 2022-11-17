using System.Collections.Generic;
using UnityEngine;

public class RockBullet : Bullet {

    private Explosion _currentExplosion;

    [Header("Explosion")]
    [SerializeField]
    private Explosion _explosion;
    [SerializeField]
    private float _radiusExplosionCollider;
    [SerializeField]
    private float _explosionDamage;
    [SerializeField]
    private int _explosionFrameRateInDestroyAnimation;
    [SerializeField]
    private int chanceExplosion;

    public bool isExplosion = false;

    private new void Start() {
        base.Start();
    }

    private new void Update() {
        base.Update();
    }

    protected override void Rotation() {
        _nextPosition = Bezier.GetTrajectoryForBullet(_bezierPoints[0].transform.position,
                       _bezierPoints[1].transform.position, _bezierPoints[2].transform.position, _t + 0.7f);
        Vector2 _moveDirection = _nextPosition - transform.position;
        float _angle = Mathf.Atan2(_moveDirection.y, _moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, _angle);
    }

    protected override void SpecialAction() {
        _isApplySpecialAbility = true;
        EnableExplosion();
    }

    private void EnableExplosion() {
        if (isExplosion) {
            SetDisableExplosion();
            _currentExplosion.transform.position = new Vector3(_targetPosition.x, _targetPosition.y);
            _currentExplosion.SetParametrsToDefault();
            SoundManager.Instance.PlaySoundEffect(SoundName.Explosion);
        }
    }

    private void SetDisableExplosion() {
        for (int i = 0; i < _bulletAbilities.Count; i++) {
            if(_bulletAbilities[i].gameObject.activeSelf == false) {
                _currentExplosion = (Explosion)_bulletAbilities[i];
                return;
            }
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.TryGetComponent(out Enemy enemy)) {
            if (_target == enemy) {
                //_target.LastPosition -= SetTargetPosition;
                _hitEnemy.Play();
                _target.Debuff.TakeDamage(_damage);
                _circleCollider.enabled = false;
                CollisionTarget(enemy);
            }
        }
    }
}
