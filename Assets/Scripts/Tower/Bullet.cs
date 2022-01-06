using System;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    private Enemy _target;
    private AnimationEvent _bulletEvent = new AnimationEvent();
    private float _axiYTower;
    private Vector3 _nexPosition;
    private bool _isBeizerPointNotNull = true;
    private Vector2 _lastTargetPosition;
    private bool _isChangeTarget = false;

    [SerializeField]
    private int _damage;
    public int Damage { get => _damage; set => _damage = value; }
    [SerializeField]
    private int _damageBasic;
    [SerializeField]
    private GameObject _bezierPoint;
    [SerializeField]
    private AnimationClip _animationClip;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private List<GameObject> _bezierPoints = new List<GameObject>();
    public float t;
    [SerializeField]
    private float _time;
    [SerializeField]
    private float _timeFlight;
    [SerializeField]
    private float _axisYP1;
    [SerializeField]
    private float _amountBezierPoints;

    public event Action DestroyBeizerPoint;

    public void SetTower(Tower tower) {
        _axiYTower = tower.transform.position.y;
    }

    public void SetTarget(Enemy enemy) {
        _target = enemy;
    }

    private void OnEnable() {
        InstantiatePointsForBeizerTrajectory();

        SetP0();

        SubscribeToEventInTheEndDestroyAnimation();
    }

    private void InstantiatePointsForBeizerTrajectory() {
        for (int i = 0; i < _amountBezierPoints; i++) {
            GameObject _point = Instantiate(_bezierPoint);
            BezierPoint bezierPoint = _point.GetComponent<BezierPoint>();
            bezierPoint.SetBullet(this);
            _point.name = "point " + i;
            _bezierPoints.Add(_point);
        }
    }

    private void SubscribeToEventInTheEndDestroyAnimation() {
        float _playingAnimationTime = _animationClip.length;
        _bulletEvent.time = _playingAnimationTime;
        _bulletEvent.functionName = nameof(DestroyBulletAndBeizerPoints);

        _animationClip.AddEvent(_bulletEvent);
    }

    private void Start() {
        _target.LastPosition += SetLastPositionTarget;
    }

    private void SetLastPositionTarget() {
        _lastTargetPosition = _target.lastPosition;
        _isChangeTarget = true;
        _target.LastPosition -= SetLastPositionTarget;
    }

    private void Update() {
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

    private void SetP1() {
        float _axisX = (_bezierPoints[0].transform.position.x - _bezierPoints[2].transform.position.x) / 2;
        _bezierPoints[1].transform.position = new Vector2(_bezierPoints[0].transform.position.x - _axisX, _axiYTower + _axisYP1);
    }

    private void SetP2() {
        if (_target != null && !_isChangeTarget) {
            _bezierPoints[2].transform.position = _target.gameObject.transform.position;
        }
        else if (_isChangeTarget) {
            _bezierPoints[2].transform.position = _lastTargetPosition;
        }
    }

    private void CalculationT() {
        _time += Time.deltaTime;
        t = _time / _timeFlight;
    }

    private void Move() {
        transform.position = Bezier.GetTrajectoryForBullet(_bezierPoints[0].transform.position,
                _bezierPoints[1].transform.position, _bezierPoints[2].transform.position, t);
    }

    private void Rotation() {
        _nexPosition = Bezier.GetTrajectoryForBullet(_bezierPoints[0].transform.position,
                       _bezierPoints[1].transform.position, _bezierPoints[2].transform.position, t + 0.1f);
        Vector2 moveDirection = _nexPosition - transform.position;
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void CheckTAndDestroyBullet() {
        if (t >= 1) {
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
                //print("bullet damage " + gameObject.name);
                _target.TakeDamage(Damage);
                _lastTargetPosition = _target.transform.position;
                _isChangeTarget = true;
            }
        }
    }

    private void PlayAnimationDestroy() {
        _animator.SetTrigger("destroy");
    }

    private void DestroyBulletAndBeizerPoints() {
        _bezierPoints.Clear();
        //print("points destroy = " + gameObject.name);
        DestroyBeizerPoint?.Invoke();
        Destroy(gameObject);
    }

    private void OnDrawGizmos() {
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
