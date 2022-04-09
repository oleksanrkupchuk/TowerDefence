using UnityEngine;

public class RockBullet : Bullet {
    private AnimationEvent _explosionEvent = new AnimationEvent();

    [Header("Explosion")]
    [SerializeField]
    private GameObject _explosionObject;
    [SerializeField]
    private float _radiusExplosionCollider;
    [SerializeField]
    private float _explosionDamage;
    [SerializeField]
    private int _explosionFrameRateInDestroyAnimation;

    public bool isExplosion = false;
    public int chanceExplosion;

    private new void Start() {
        base.Start();
    }

    private new void Update() {
        base.Update();
    }

    protected override void Rotation() {
        _nexPosition = Bezier.GetTrajectoryForBullet(_bezierPoints[0].transform.position,
                       _bezierPoints[1].transform.position, _bezierPoints[2].transform.position, _t + 0.7f);
        Vector2 _moveDirection = _nexPosition - transform.position;
        float _angle = Mathf.Atan2(_moveDirection.y, _moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, _angle);
    }

    public void AddEventExplosionForDestroyAnimation() {
        float _playingAnimationTime = _explosionFrameRateInDestroyAnimation / _destroyClip.frameRate;
        _explosionEvent.time = _playingAnimationTime;
        _explosionEvent.functionName = nameof(SetSizeExplosionCollider);

        _destroyClip.AddEvent(_explosionEvent);
    }

    protected void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.TryGetComponent(out Enemy enemy)) {
            _tower.RemoveBullet(this);
            if (_target == enemy) {
                _target.LastPosition -= SetTargetPosition;
                _target.TakeDamage(_damage);
                DisableCircleCollider();
                SetTargetPositionAndSetTargetNull();
            }

            int _chance = Random.Range(0, 100);

            if (isExplosion && _chance <= chanceExplosion) {
                enemy.TakeDamage(_explosionDamage);
            }
        }
    }

    private void SetSizeExplosionCollider() {
        if (isExplosion) {
            _circleCollider.radius = _radiusExplosionCollider;
            EnableCircleCollider();
            SoundManager.Instance.PlaySoundEffect(SoundName.Explosion);
        }
    }
}
