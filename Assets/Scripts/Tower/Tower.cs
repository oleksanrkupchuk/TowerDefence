using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    [Header("Parametrs")]
    [SerializeField]
    private float _rangeAttack;
    [SerializeField] 
    private float timeShoot;
    [SerializeField] 
    private float timer;
    [SerializeField] 
    private Transform _bulletPosition;
    [SerializeField]
    private int _stepDegree;
    [SerializeField]
    private int _price;
    public int Price { get => _price; }
    public int numberUpgrade = 3;

    [Header("Components")]
    [SerializeField] 
    private BoxCollider2D _boxCollider;
    [SerializeField] 
    private GameObject _buletPrefab;
    [SerializeField]
    private LineRenderer _lineRenderer;
    private GameObject _bullet;
    private TowerManager _towerManager;

    private Enemy _target;
    private ListEnemys _enemyList;

    //tower menu
    [SerializeField]
    private Image _upgrade;
    [SerializeField]
    private Image _sell;
    [SerializeField]
    private Image _repair;

    [SerializeField]
    private GameObject _upgradeObject;
    [SerializeField]
    private GameObject _sellObject;
    [SerializeField]
    private GameObject _repairObject;


    private void Awake()
    {
        _towerManager = FindObjectOfType<TowerManager>();
        _lineRenderer.enabled = false;
        _enemyList = FindObjectOfType<ListEnemys>();
        SetRangeRadius(_rangeAttack);
        _towerManager._towers.Add(GetComponent<Tower>());
        SetTowerMenuIcon(_rangeAttack);
    }

    private void SetRangeRadius(float radius) {
        int _countStep = 360 / _stepDegree;

        _lineRenderer.positionCount = _countStep;

        float _dotX;
        float _dotY;

        float _degree = 0;

        Vector3[] _positionPoints = new Vector3[_countStep];

        for (int i = 0; i < _countStep; i++) {

            float _degreeInRadians = _degree * Mathf.PI / 180;
            _dotX = transform.position.x + radius * Mathf.Cos(_degreeInRadians);
            _dotY = transform.position.y + radius * Mathf.Sin(_degreeInRadians);

            _positionPoints[i] = new Vector3(_dotX, _dotY, -7);

            _degree += _stepDegree;
        }

        _lineRenderer.SetPositions(_positionPoints);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeShoot) {
            FindingTarget();

            if (_target != null) {
                Shoot(_target);
            }

            timer = 0;
        }
    }

    private void FindingTarget() {
        _target = null;
        float _nearestEnemyDistance = Mathf.Infinity;

        foreach (Enemy enemy in _enemyList.ListEnemy) {
            float _currentDistance = Vector2.Distance(transform.position, enemy.transform.position);
            //print("----------------------------------------");
            //print("_rangeAttack = " + _rangeAttack);

            if(_currentDistance < _nearestEnemyDistance && _currentDistance <= _rangeAttack) {
                //_target = enemy.transform.GetComponent<Enemy>();
                _target = enemy;
                _nearestEnemyDistance = _currentDistance;

            }
        }
    }

    private void SetTowerMenuIcon(float range) {
        _upgradeObject.transform.localPosition = new Vector3(0f, range);
        _repair.transform.localPosition = new Vector3(range, 0f);
        _sell.transform.localPosition = new Vector3(0f, -range);
    }

    private void Shoot(Enemy target)
    {
        _bullet = Instantiate(_buletPrefab, _bulletPosition.position, Quaternion.identity);

        Collider2D _bulletCollider = _bullet.GetComponent<CircleCollider2D>();

        Physics2D.IgnoreCollision(_boxCollider, _bulletCollider, true);

        _bullet.GetComponent<Bullet>().SetTarget(target);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Tags.bullet)) {
            Destroy(collision.gameObject);
        }
    }

    public void UpgradeTower(float additionalRangeAttack) {
        _rangeAttack += additionalRangeAttack;
        SetRangeRadius(_rangeAttack);
    }

    public void EnableLineRenderer(bool isActive) {
        _lineRenderer.enabled = isActive;
    }

    public void EnableTowerMenu(bool isActive) {
        _upgrade.enabled = isActive;
        _sell.enabled = isActive;
        _repair.enabled = isActive;
    }
}
