using System;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    private Enemy _target;
    private AnimationEvent _bulletEvent = new AnimationEvent();
    private float _axiYTower;
    private Vector3 _nexPosition;
    private bool _isBeizerPointNotNull = true;

    [SerializeField]
    private int _damage;
    public int Damage { get => _damage; set => _damage = value; }
    [SerializeField]
    private int _damageBasic;
    [SerializeField]
    private GameObject _pointObject;
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
    }

    private void InstantiatePointsForBeizerTrajectory() {
        for (int i = 0; i < _amountBezierPoints; i++) {
            GameObject _point = Instantiate(_pointObject);
            BezierPoint bezierPoint = _point.GetComponent<BezierPoint>();
            bezierPoint.SetBullet(this);
            _point.name = "point " + i;
            _bezierPoints.Add(_point);
        }
    }

    private void Update() {
        if (_isBeizerPointNotNull) {
            SetP2();
            SetP1(_bezierPoints[0].transform, _bezierPoints[2].transform);

            CalculationT();

            Move();
            Rotation();
        }

    }

    public void SetP0() {
        _bezierPoints[0].transform.position = transform.position;
    }

    private void SetP1(Transform p0, Transform p2) {
        if (_bezierPoints.Count > 0) {
            if (p2 != null) {
                float _axisX = (p0.transform.position.x - p2.transform.position.x) / 2;
                _bezierPoints[1].transform.position = new Vector2(p0.transform.position.x - _axisX, _axiYTower + _axisYP1);
            }
            else {
                print("p2 is null");
            }
        }
    }

    private void SetP2() {
        if (_bezierPoints.Count > 0) {
            if (_bezierPoints[2] != null) {
                _bezierPoints[2].transform.position = _target.gameObject.transform.position;
            }
        }
    }

    private void CalculationT() {
        _time += Time.deltaTime;
        t = _time / _timeFlight;
    }

    private void Move() {
        if (_target != null && _bezierPoints.Count > 0) {
            transform.position = Bezier.GetTrajectoryForBullet(_bezierPoints[0].transform.position,
                _bezierPoints[1].transform.position, _bezierPoints[2].transform.position, t);
        }
    }

    private void Rotation() {
        if (_target != null) {
            _nexPosition = Bezier.GetTrajectoryForBullet(_bezierPoints[0].transform.position,
                        _bezierPoints[1].transform.position, _bezierPoints[2].transform.position, t + 0.1f);
            Vector2 moveDirection = _nexPosition - transform.position;
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    public void SetBasicDamage() {
        _damage = _damageBasic;
    }

    public void StopCalculation() {
        _isBeizerPointNotNull = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag(Tags.enemy)) {
            _target.TakeDamage(Damage);
            PlayDestroyAnimationAndSubscribeToEventInTheEndAnimation();
        }
    }

    private void PlayDestroyAnimationAndSubscribeToEventInTheEndAnimation() {
        float _playingAnimationTime = _animationClip.length;
        _bulletEvent.time = _playingAnimationTime;
        _bulletEvent.functionName = nameof(DestroyBulletAndBeizerPoints);

        _animationClip.AddEvent(_bulletEvent);

        _animator.SetTrigger("destroy");
    }

    private void DestroyBulletAndBeizerPoints() {
        _bezierPoints.Clear();
        DestroyBeizerPoint?.Invoke();
        Destroy(gameObject);
    }

    //private void OnDrawGizmos() {
    //    if (_target != null && _bezierPoint[0] != null && _bezierPoint[1] != null) {
    //        int sigmentNumber = 60;
    //        Vector2 previousPoint = _bezierPoint[0].transform.position;

    //        for (int i = 0; i < sigmentNumber; i++) {
    //            float param = (float)i / sigmentNumber;
    //            Vector2 point = Bezier.GetTrajectoryForBullet(_bezierPoint[0].transform.position,
    //                _bezierPoint[1].transform.position, _bezierPoint[2].transform.position, param);
    //            Gizmos.DrawLine(previousPoint, point);
    //            previousPoint = point;
    //        }
    //    }
    //    else {
    //        print("p2 is null");
    //    }
    //}
}
