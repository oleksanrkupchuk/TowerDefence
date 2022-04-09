using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour {
    protected Tower _tower;
    [SerializeField]
    protected Enemy _target;
    protected float _axiYTower;
    protected Vector3 _nexPosition;
    protected bool _isBeizerPointNotNull = true;
    protected Vector2 _targetPosition;
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

    public int Damage { get => _damage; }

    public void Init(Tower tower) {
        _tower = tower;
    }

    public void SetParametrs() {
        transform.position = _tower._bulletPosition.position;
        _axiYTower = _tower.transform.position.y;
        _target = _tower.Target;
        _target.LastPosition += SetTargetPosition;
    }

    public void SetParametrs(Transform targetPosition) {
        transform.position = _tower._bulletPosition.position;
        _axiYTower = _tower.transform.position.y;
        _targetPosition = targetPosition.position;
    }

    protected void OnEnable() {
        _t = 0;
        _timeWay = 0;
    }

    protected void SetTargetPosition() {
        //_circleCollider.enabled = false;

        _targetPosition = _target.transform.position;
        _target.LastPosition -= SetTargetPosition;
        _target = null;
    }

    public void SetNullTarget() {
        if (_target != null) {
            _target.LastPosition -= SetTargetPosition;
        }
        _circleCollider.enabled = false;

        _targetPosition = _tower.GetTargetPosition().position;
        //_targetPosition = _target.transform.position;
        _target = null;
    }

    protected void Start() {
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
        if (_isBeizerPointNotNull) {
        }
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
        _nexPosition = Bezier.GetTrajectoryForBullet(_bezierPoints[0].transform.position,
                       _bezierPoints[1].transform.position, _bezierPoints[2].transform.position, _t + 0.1f);
        Vector2 _moveDirection = _nexPosition - transform.position;
        float _angle = Mathf.Atan2(_moveDirection.y, _moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, _angle);
    }

    protected virtual void CheckTAndDestroyBullet() {
        if (_t >= 1) {
            PlayAnimationDestroy();
        }
    }

    public void SetDamage(int value) {
        _damage = value;
    }

    public void ResetDamage() {
        _damage = _damageBasic;
    }

    public void SetBezierPointsNull() {
        _isBeizerPointNotNull = false;
    }

    protected void PlayAnimationDestroy() {
        _animator.SetTrigger("destroy");
    }

    protected void SetTargetPositionAndSetTargetNull() {
        _targetPosition = _target.transform.position;
        _target = null;
    }

    protected virtual void DestroyBulletAfterTime() {
        Destroy(gameObject);
    }

    protected void EnableCircleCollider() {
        _circleCollider.enabled = true;
    }

    protected void DisableCircleCollider() {
        _circleCollider.enabled = false;
    }

    public void SetDistanceAndRange(float range) {
        _timeFlight = _speed / range;
    }

    protected void OnDrawGizmos() {
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
}
