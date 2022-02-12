using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour {
    [SerializeField]
    protected Enemy _target;
    protected float _axiYTower;
    protected Vector3 _nexPosition;
    protected Vector3 _previousPosition;
    protected bool _isBeizerPointNotNull = true;
    protected Vector2 _targetPosition;
    protected float _t;
    protected float _testDistance;
    protected float _timeTest;
    protected float _timeTest2;
    protected float _timeFormula1;
    protected float _timeFormula2;
    protected float _timeFormulaBuffer;
    protected float _timeDeltaTime;
    protected float _timeWay = 0f;
    protected AnimationEvent _destroyEvent = new AnimationEvent();
    protected List<GameObject> _bezierPoints = new List<GameObject>();

    [SerializeField]
    protected int _damage;
    [SerializeField]
    protected int _damageBasic;
    [SerializeField]
    protected float _speed;
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
    protected CircleCollider2D _circleCollider;

    public int Damage { get => _damage; set => _damage = value; }
    public event Action DestroyBeizerPoint;

    public void Init(Tower tower) {
        _axiYTower = tower.transform.position.y;
        _target = tower.GetTarget();
        _target.LastPosition += SetTargetPosition;
    }

    public void Init(Tower tower, Transform targetPosition) {
        _axiYTower = tower.transform.position.y;
        _targetPosition = targetPosition.position;
    }

    protected void SetTargetPosition() {
        //print("set pos + " + gameObject.name);
        _circleCollider.enabled = false;
        _targetPosition = _target.transform.position;
        _target.LastPosition -= SetTargetPosition;
        _target = null;
    }

    protected void OnEnable() {
        //print("on enable bullet");
        //InstantiatePointsForBeizerTrajectory();

        //SetP0();
    }

    private void Start() {
        //print("start bullet");
        InstantiatePointsForBeizerTrajectory();

        SetP0();
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

    public void AddDestroyEventForDestroyAnimation() {
        float _playingAnimationTime = _destroyBullet.length;
        _destroyEvent.time = _playingAnimationTime;
        _destroyEvent.functionName = nameof(DestroyBulletAndBeizerPoints);

        _destroyBullet.AddEvent(_destroyEvent);
    }

    protected void Update() {
        if (_isBeizerPointNotNull) {
            SetP2();
            SetP1();

            CalculationT();

            Move();
            Rotation();
            CheckTAndDestroyBullet();
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
        if (_target != null) {
            //print("bullet have target");
            _bezierPoints[2].transform.position = _target.transform.position;
        }
        else {
            _bezierPoints[2].transform.position = _targetPosition;
        }
    }

    protected void CalculationT() {
        _previousPosition = Bezier.GetTrajectoryForBullet(_bezierPoints[0].transform.position,
                       _bezierPoints[1].transform.position, _bezierPoints[2].transform.position, _t - 0.1f);
        _timeTest += Time.deltaTime;
        _timeTest2 += Time.deltaTime;

        _timeFormula1 += Time.deltaTime;

        if (transform.position.y > _previousPosition.y) {
            _timeFormulaBuffer = 1 / (1 + _timeFormula1) * _timeFormula1 * 1.2f;
            _t = _timeFormulaBuffer;
        }
        else {
            _timeFormula2 += Time.deltaTime;
            //_t = _timeFormulaBuffer + (_timeFormula2 * _timeFormula2 * 1.5f);
            _t = _timeFormulaBuffer + (_timeFormula2 * _timeFormula2 * 2.5f);
        }

        _timeFlight = _testDistance / _speed;
    }

    protected void Move() {
        transform.position = Bezier.GetTrajectoryForBullet(_bezierPoints[0].transform.position,
                _bezierPoints[1].transform.position, _bezierPoints[2].transform.position, _t);
    }

    protected void Rotation() {
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

    protected void DestroyBulletAndBeizerPoints() {
        _bezierPoints.Clear();
        DestroyBeizerPoint?.Invoke();
        Destroy(gameObject);
    }

    protected void EnableCircleCollider() {
        _circleCollider.enabled = true;
    }

    protected void DisableCircleCollider() {
        _circleCollider.enabled = false;
    }

    public void SetDistanceAndRange(float distance, float range) {
        _testDistance = range;
        //if (Mathf.Abs(distance - range) >= 0.5f && Mathf.Abs(distance - range) < 1f) {
        //    _timeFlight = (_speed / range) - 0.1f;
        //    print("time flight = " + _timeFlight);
        //    return;
        //}
        //if (Mathf.Abs(distance - range) >= 1f && Mathf.Abs(distance - range) < 1.5f) {
        //    _timeFlight = (_speed / range) - 0.15f;
        //    print("time flight = " + _timeFlight);
        //    return;
        //}
        //if (Mathf.Abs(distance - range) >= 1.5f) {
        //    _timeFlight = (_speed / range) - 0.3f;
        //    print("time flight = " + _timeFlight);
        //    return;
        //}

        _timeFlight = _speed / range;
        //print("time flight = " + _timeFlight);
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
