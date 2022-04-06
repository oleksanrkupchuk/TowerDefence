using UnityEngine;

public class RockBullet : Bullet {
    private AnimationEvent _explosionEvent = new AnimationEvent();

    [Header("Explosion")]
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

    protected override void CalculationT() {
        _previousPosition = Bezier.GetTrajectoryForBullet(_bezierPoints[0].transform.position,
                       _bezierPoints[1].transform.position, _bezierPoints[2].transform.position, _t - 0.1f);

        _timeFormula1 += Time.deltaTime;

        if (transform.position.y > _previousPosition.y) {
            //_timeFormulaBuffer = 1 / (1 + _timeFormula1) * _timeFormula1 * 1.5f;
            _timeFormulaBuffer = 1 / (1 + _timeFormula1) * _timeFormula1;
            _t = _timeFormulaBuffer;
        }
        else {
            _timeFormula2 += Time.deltaTime;
            //_t = _timeFormulaBuffer + _timeFormula2;
            _t = _timeFormulaBuffer + (_timeFormula2 * _timeFormula2 * 1.5f);
            //_t = _timeFormulaBuffer + (_timeFormula2 * _timeFormula2 * 2.5f);
            //_t = _timeFormulaBuffer + (_timeFormula2 + _timeFormula2 * _timeFormula2);
        }
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
