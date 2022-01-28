using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour {
    protected Enemy _target;
    protected AnimationEvent _bulletEvent = new AnimationEvent();
    protected float _axiYTower;
    protected Vector3 _nexPosition;
    protected bool _isBeizerPointNotNull = true;
    protected Vector2 _lastTargetPosition;
    protected bool _isChangeTarget = false;
    protected float _t;
    protected float _time;
    protected List<GameObject> _bezierPoints = new List<GameObject>();

    [SerializeField]
    protected int _damage;
    [SerializeField]
    protected int _damageBasic;
    [SerializeField]
    protected GameObject _bezierPointPrefab;
    [SerializeField]
    protected int _amountBezierPoints;
    [SerializeField]
    protected AnimationClip _destroyBullet;
    [SerializeField]
    protected Animator _animator;
    [SerializeField]
    protected float _timeFlight;
    [SerializeField]
    protected float _axisYP1;
    [SerializeField]
    protected Collider2D _circleCollider;

    public int Damage { get => _damage; set => _damage = value; }
    public event Action DestroyBeizerPoint;

    public void SetTower(Tower tower) {
        _axiYTower = tower.transform.position.y;
    }

    public void SetTarget(Enemy enemy) {
        _target = enemy;
    }

    protected void OnEnable() {
        InstantiatePointsForBeizerTrajectory();

        SetP0();

        SubscribeToEventInTheEndDestroyAnimation();
    }

    protected void InstantiatePointsForBeizerTrajectory() {
        for (int i = 0; i < _amountBezierPoints; i++) {
            GameObject _point = Instantiate(_bezierPointPrefab);
            BezierPoint bezierPoint = _point.GetComponent<BezierPoint>();
            bezierPoint.SetBullet(this);
            _point.name = "point " + i;
            _bezierPoints.Add(_point);
        }
    }

    protected void SubscribeToEventInTheEndDestroyAnimation() {
        float _playingAnimationTime = _destroyBullet.length;
        _bulletEvent.time = _playingAnimationTime;
        _bulletEvent.functionName = nameof(DestroyBulletAndBeizerPoints);

        _destroyBullet.AddEvent(_bulletEvent);
    }

    protected void Start() {
        _target.LastPosition += SetLastPositionTarget;
    }

    protected void SetLastPositionTarget() {
        _lastTargetPosition = _target.lastPosition;
        _isChangeTarget = true;
        _target.LastPosition -= SetLastPositionTarget;
    }

    protected void Update() {
        if (_isBeizerPointNotNull) {
            SetP2();
            SetP1();

            CalculationT();

            Move();
            Rotation();
            CheckTAndDestroyBullet();
            //print(gameObject.name + "target = " + _target);
            //print(gameObject.name + " p2 = " + _bezierPoints[2].transform.position);
        }
    }

    public void SetP0() {
        _bezierPoints[0].transform.position = transform.position;
    }

    protected void SetP1() {
        float _axisX = (_bezierPoints[0].transform.position.x - _bezierPoints[2].transform.position.x) / 2;
        _bezierPoints[1].transform.position = new Vector2(_bezierPoints[0].transform.position.x - _axisX, _axiYTower + _axisYP1);
    }

    protected void SetP2() {
        if (_target != null && !_isChangeTarget) {
            _bezierPoints[2].transform.position = _target.gameObject.transform.position;
        }
        else if (_isChangeTarget) {
            _bezierPoints[2].transform.position = _lastTargetPosition;
        }
    }

    protected void CalculationT() {
        _time += Time.deltaTime;
        _t = _time / _timeFlight;
    }

    protected void Move() {
        transform.position = Bezier.GetTrajectoryForBullet(_bezierPoints[0].transform.position,
                _bezierPoints[1].transform.position, _bezierPoints[2].transform.position, _t);
    }

    protected void Rotation() {
        _nexPosition = Bezier.GetTrajectoryForBullet(_bezierPoints[0].transform.position,
                       _bezierPoints[1].transform.position, _bezierPoints[2].transform.position, _t + 0.1f);
        Vector2 moveDirection = _nexPosition - transform.position;
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    protected void CheckTAndDestroyBullet() {
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

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag(Tags.enemy)) {
            if (_target != null) {
                _circleCollider.enabled = false;
                //print("bullet" + gameObject.name + " = " + "damage " + _damage);
                _target.TakeDamage(_damage);
                _lastTargetPosition = _target.transform.position;
                _isChangeTarget = true;
            }
        }
    }

    protected void PlayAnimationDestroy() {
        _animator.SetTrigger("destroy");
    }

    protected void DestroyBulletAndBeizerPoints() {
        _bezierPoints.Clear();
        //print("points destroy = " + gameObject.name);
        DestroyBeizerPoint?.Invoke();
        Destroy(gameObject);
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
