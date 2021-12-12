using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    [SerializeField]
    private int _damage;
    public int Damage { get => _damage; set => _damage = value; }
    [SerializeField]
    private int _damageBasic;

    private Enemy _target;
    [SerializeField]
    private GameObject _pointObject;
    [SerializeField]
    private List<GameObject> _bezierPoint = new List<GameObject>();

    private float _axiYTower;
    private Vector3 _nexPosition;

    public float t;
    [SerializeField]
    private float _time;
    [SerializeField]
    private float _timeFlight;
    [SerializeField]
    private float _axisYP1;
    [SerializeField]
    private float _amountBezierPoints;

    public void SetTower(Tower tower) {
        _axiYTower = tower.transform.position.y;
    }

    private void OnEnable() {
        InstantiatePointsForBeizerTrajectory();

        SetP0();
    }

    private void InstantiatePointsForBeizerTrajectory() {
        for (int i = 0; i < _amountBezierPoints; i++) {
            GameObject _point = Instantiate(_pointObject);
            _point.name = "point " + i;
            _bezierPoint.Add(_point);
        }
    }

    private void Update() {
        SetP2();
        SetP1(_bezierPoint[0].transform, _bezierPoint[2].transform);

        CalculationT();

        Move();
        Rotation();
    }

    public void SetP0() {
        _bezierPoint[0].transform.position = transform.position;
    }

    private void SetP1(Transform p0, Transform p2) {
        if (p2 != null) {
            float _axisX = (p0.transform.position.x - p2.transform.position.x) / 2;
            _bezierPoint[1].transform.position = new Vector2(p0.transform.position.x - _axisX, _axiYTower + _axisYP1);
        }
        else {
            print("p2 is null");
        }
    }

    private void SetP2() {
        _bezierPoint[2].transform.position = _target.gameObject.transform.position;
    }

    private void Move() {
        if (_target != null) {
            transform.position = Bezier.GetTrajectoryForBullet(_bezierPoint[0].transform.position,
                _bezierPoint[1].transform.position, _bezierPoint[2].transform.position, t);
        }
    }

    private void Rotation() {
        if (_target != null) {
            _nexPosition = Bezier.GetTrajectoryForBullet(_bezierPoint[0].transform.position,
                        _bezierPoint[1].transform.position, _bezierPoint[2].transform.position, t + 0.1f);
            Vector2 moveDirection = _nexPosition - transform.position;
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    private void CalculationT() {
        _time += Time.deltaTime;
        t = _time / _timeFlight;
    }

    public void SetTarget(Enemy enemy) {
        _target = enemy;
    }

    public void SetBasicDamage() {
        _damage = _damageBasic;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag(Tags.enemy)) {
            _target.TakeDamage(Damage);
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos() {
        if (_target != null) {
            int sigmentNumber = 60;
            Vector2 previousPoint = _bezierPoint[0].transform.position;

            for (int i = 0; i < sigmentNumber; i++) {
                float param = (float)i / sigmentNumber;
                Vector2 point = Bezier.GetTrajectoryForBullet(_bezierPoint[0].transform.position,
                    _bezierPoint[1].transform.position, _bezierPoint[2].transform.position, param);
                Gizmos.DrawLine(previousPoint, point);
                previousPoint = point;
            }
        }
        else {
            print("p2 is null");
        }
    }
}
