using UnityEngine;

public class RockBullet : Bullet {
    private bool _isExplosion = false;
    private AnimationEvent _explosionEvent = new AnimationEvent();

    [SerializeField]
    private float _radiusExplosionCollider;
    [SerializeField]
    private float _explosionDamage;
    [SerializeField]
    private int _explosionFrameRateInDestroyAnimation;
    [SerializeField]
    private AnimationClip _destroyClip;

    private new void OnEnable() {
        base.OnEnable();
    }

    private new void Update() {
        base.Update();
    }

    public void AddEventExplosionForDestroyAnimation() {
        float _playingAnimationTime = _explosionFrameRateInDestroyAnimation / _destroyClip.frameRate; ;
        _explosionEvent.time = _playingAnimationTime;
        _explosionEvent.functionName = nameof(SetSizeExplosionCollider);

        _destroyClip.AddEvent(_explosionEvent);
    }

    protected void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.TryGetComponent(out Enemy enemy)) {
            if (_target == enemy) { //&& !_isExplosion
                _target.LastPosition -= SetTargetPosition;
                _target.TakeDamage(_damage);
                DisableCircleCollider();
                SetTargetPositionAndSetTargetNull();
            }
            print("enemy = " + enemy.gameObject.name);
            if (_isExplosion) {
                enemy.TakeDamage(_explosionDamage);
            }
        }
    }

    private void SetSizeExplosionCollider() {
        _isExplosion = true;
        _circleCollider.radius = _radiusExplosionCollider;
        EnableCircleCollider();
    }
}
