using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour {
    protected bool _isApplySpecialAbility;
    protected Tower _tower;

    [SerializeField]
    public Enemy _target;
    public Vector2 _targetPosition;

    protected float _axiYTower;
    protected Vector3 _nextPosition;
    protected bool _isBeizerPointNotNull = true;
    protected float _t;
    protected float _timeWay = 0f;
    protected AnimationEvent _destroyEvent = new AnimationEvent();
    [SerializeField]
    protected List<GameObject> _bezierPoints = new List<GameObject>();

    [SerializeField]
    protected int _damage;
    [SerializeField]
    protected int _damageBasic;
    [SerializeField]
    protected float _speed;
    [SerializeField]
    protected GameObject _bezierPoint;
    [SerializeField]
    protected AnimationClip _destroyClip;
    [SerializeField]
    protected Animator _animator;
    [SerializeField]
    protected float _timeFlight;
    [SerializeField]
    protected float _axisYP1;
    [SerializeField]
    protected CircleCollider2D _circleCollider;
    [SerializeField]
    protected List<BulletAbility> _bulletAbilities = new List<BulletAbility>();

    public int Damage { get => _damage; }

    public void Init(Tower tower) {
        _tower = tower;
        _bulletAbilities = _tower.BulletsAbility;
    }

    public void SetDefaulPositionBulletAndTarget() {
        SetStartPositionBullet();

        _target = _tower.Target;
    }

    public void SetDefaulPositionBulletAndTargetPosition(Transform position) {
        SetStartPositionBullet();

        _targetPosition = position.position;
    }

    private void SetStartPositionBullet() {
        transform.position = _tower.BulletPosition.position;
        _axiYTower = _tower.transform.position.y;
    }

    protected void OnEnable() {
        _t = 0;
        _timeWay = 0;
        _isApplySpecialAbility = false;
    }

    public void CollisionTarget(Enemy enemy) {
        if (_target != null) {
            SetTartgetNullAndLastPosition(enemy);
        }
    }

    protected void EnemyDead(Enemy enemy) {
        //_circleCollider.enabled = false;
        if (_target == enemy) {
            SetTartgetNullAndLastPosition(enemy);
        }
    }

    public void EnemyOutRangeTower(Enemy enemy) {
        if (_target == enemy) {
            SetTartgetNullAndLastPosition(enemy);
        }
    }

    protected void SetTartgetNullAndLastPosition(Enemy enemy) {
        _targetPosition = enemy.transform.position;
        _target = null;
    }

    protected void Start() {
        Enemy.Dead += EnemyDead;
        DisableCollider();

        SetP0();
    }

    public void SetP0() {
        _bezierPoints[0].transform.position = transform.position;
    }

    public void CreatePointsForBeizerTrajectory(Transform parent) {
        for (int i = 0; i < 3; i++) {
            GameObject _point = Instantiate(_bezierPoint);
            _point.name = "point " + i;
            _point.transform.SetParent(parent);
            _bezierPoints.Add(_point.gameObject);
        }
    }

    protected void DisableCollider() {
        _circleCollider.enabled = false;
    }

    public void AddDestroyEventForDestroyAnimation() {
        float _playingAnimationTime = _destroyClip.length;
        _destroyEvent.time = _playingAnimationTime;
        _destroyEvent.functionName = nameof(DestroyBulletAndBeizerPoints);

        _destroyClip.AddEvent(_destroyEvent);
    }

    protected void DestroyBulletAndBeizerPoints() {
        //_tower.RemoveBullet(this);
        gameObject.SetActive(false);
    }

    protected void Update() {
        SetP2();
        SetP1();

        CalculationT();
        EnableCollider();

        Move();
        Rotation();
        CheckTAndDestroyBullet();
    }

    protected void SetP2() {
        if (_target != null) {
            _bezierPoints[2].transform.position = _target.transform.position;
        }
        else {
            _bezierPoints[2].transform.position = _targetPosition;
        }
    }

    protected void SetP1() {
        float _axisX = (_bezierPoints[0].transform.position.x - _bezierPoints[2].transform.position.x) / 2;
        _bezierPoints[1].transform.position = new Vector2(_bezierPoints[0].transform.position.x - _axisX, _axiYTower + _axisYP1);
    }

    protected void CalculationT() {
        _timeWay += Time.deltaTime;
        _t = _timeWay / _timeFlight;
    }

    protected void EnableCollider() {
        if (_circleCollider.enabled == false) {
            if (_t >= 0.8f) {
                _circleCollider.enabled = true;
            }
        }
    }

    protected void Move() {
        transform.position = Bezier.GetTrajectoryForBullet(_bezierPoints[0].transform.position,
                _bezierPoints[1].transform.position, _bezierPoints[2].transform.position, _t);
    }

    protected virtual void Rotation() {
        _nextPosition = Bezier.GetTrajectoryForBullet(_bezierPoints[0].transform.position,
                       _bezierPoints[1].transform.position, _bezierPoints[2].transform.position, _t + 0.1f);
        Vector2 _moveDirection = _nextPosition - transform.position;
        float _angle = Mathf.Atan2(_moveDirection.y, _moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, _angle);
    }

    protected void CheckTAndDestroyBullet() {
        if (_t < 1) {
            return;
        }

        if (!_isApplySpecialAbility) {
            SpecialAction();
        }

        PlayAnimationDestroy();
    }

    protected virtual void SpecialAction() {
        _isApplySpecialAbility = true;
    }

    public void SetDamage(int value) {
        _damage = value;
    }

    public void ResetDamage() {
        _damage = _damageBasic;
    }

    protected void PlayAnimationDestroy() {
        _animator.SetTrigger("destroy");
    }

    protected void DisableCircleCollider() {
        _circleCollider.enabled = false;
    }

    public void SetDistanceAndRange(float range) {
        _timeFlight = _speed / range;
    }

    protected void OnDrawGizmos() {
        if (_bezierPoints.Count == 0) {
            return;
        }

        int sigmentNumber = 60;
        Vector2 previousPoint = _bezierPoints[0].transform.position;

        for (int i = 0; i < sigmentNumber; i++) {
            float param = (float)i / sigmentNumber;
            Vector2 point = Bezier.GetTrajectoryForBullet(_bezierPoints[0].transform.position,
                _bezierPoints[1].transform.position, _bezierPoints[2].transform.position, param);
            Gizmos.DrawLine(previousPoint, point);
            previousPoint = point;
        }
    }

    protected void OnDestroy() {
        Enemy.Dead -= EnemyDead;
    }
}
