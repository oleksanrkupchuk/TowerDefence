using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour {
    [Header("Parametrs")]
    [SerializeField]
    private float _rangeAttack;
    public float RangeAttack { get => _rangeAttack; }
    [SerializeField]
    private float _timeShoot;
    private float _timer;
    [SerializeField]
    private Transform _bulletPosition;
    [SerializeField]
    private int _stepDegree;
    [SerializeField]
    private int _price;
    public int Price { get => _price; }

    [Header("Components")]
    [SerializeField]
    private GameObject _buletPrefab;
    [SerializeField]
    private Bullet _buletScript;
    public Bullet Bullet { get => _buletPrefab.GetComponent<Bullet>(); }
    [SerializeField]
    private LineRenderer _lineRenderer;
    private GameObject _bulletObject;
    private TowerManager _towerManager;
    [HideInInspector]
    public GameObject placeForTower = null;

    private Enemy _target;
    private ListEnemys _enemyList;

    private bool _isShooting = false;

    private Collider2D[] _colliders;

    [Header("Upgrades Tower")]
    [SerializeField]
    private GameObject _objectIncreaseDamage;
    [SerializeField]
    private GameObject _objectSell;
    [SerializeField]
    private GameObject _objectIncreaseRange;

    [SerializeField]
    private TowerUpgradeMenu _towerUpgradeMenu;

    private void Awake() {
        _towerManager = FindObjectOfType<TowerManager>();
        _lineRenderer.enabled = false;
        _enemyList = FindObjectOfType<ListEnemys>();
        SetRangeRadius(_rangeAttack);
        _towerManager.towersList.Add(GetComponent<Tower>());
        //SetPositionUpgradeIcon(2f);

        GetAllColliderOnTower();

        _buletScript.SetBasicDamage();

        _towerUpgradeMenu.InitializationButtonUpgrade(this);
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
            //Debug.Log($"dot x {i} = " + _dotX);
            _dotY = transform.position.y + radius * Mathf.Sin(_degreeInRadians);
            //Debug.Log($"dot y {i} = " + _dotY);

            _positionPoints[i] = new Vector3(_dotX, _dotY, -7);

            _degree += _stepDegree;
        }

        _lineRenderer.SetPositions(_positionPoints);
    }

    private void SetPositionUpgradeIcon(float distance) {
        _objectIncreaseDamage.transform.position = new Vector3(transform.position.x, transform.position.y + distance);
        _objectSell.transform.position = new Vector3(transform.position.x + distance, transform.position.y);
        _objectIncreaseRange.transform.position = new Vector3(transform.position.x - distance, transform.position.y);
    }

    private void GetAllColliderOnTower() {
        _colliders = GetComponents<Collider2D>();
    }

    void Update() {
        if (_isShooting) {
            _timer -= Time.deltaTime;
        }

        if (_timer <= 0f) {
            _isShooting = false;

            FindingTarget();

            if (_target != null) {
                Shoot(_target);
                _timer = _timeShoot;
                _isShooting = true;
            }
        }
    }

    private void FindingTarget() {
        _target = null;
        float _nearestEnemyDistance = Mathf.Infinity;

        foreach (Enemy enemy in _enemyList.ListEnemy) {
            float _currentDistance = Vector2.Distance(transform.position, enemy.transform.position);
            //print("----------------------------------------");
            //print("_rangeAttack = " + _rangeAttack);

            if (_currentDistance < _nearestEnemyDistance && _currentDistance <= _rangeAttack) {
                //_target = enemy.transform.GetComponent<Enemy>();
                _target = enemy;
                _nearestEnemyDistance = _currentDistance;

            }
        }
    }


    private void Shoot(Enemy target) {
        _bulletObject = Instantiate(_buletPrefab, _bulletPosition.position, Quaternion.identity);

        Collider2D _bulletCollider = _bulletObject.GetComponent<CircleCollider2D>();

        IgnoreCollisionCollidersTowerAndBullet(_bulletCollider);

        _bulletObject.GetComponent<Bullet>().SetTarget(target);
    }

    private void IgnoreCollisionCollidersTowerAndBullet(Collider2D bulletCollider) {
        for (int count = 0; count < _colliders.Length; count++) {
            Physics2D.IgnoreCollision(_colliders[count], bulletCollider, true);
        }
    }

    public void IncreaseDamage() {
        _buletScript.Damage += Damage();
        print("tower damage = " + _buletScript.Damage);
    }

    private int Damage() {
        int damage = 0;
        damage += _buletScript.Damage / 2;
        if(damage < 1) {
            damage = 1;
        }
        return damage;
    }

    public void IncreaseRange(float range) {
        //print("Range up");
        _rangeAttack += range;
        SetRangeRadius(_rangeAttack);
    }

    public void EnableLineRenderer() {
        _lineRenderer.enabled = true;
    }

    public void DisableLineRenderer() {
        _lineRenderer.enabled = false;
    }

    public void EnableTowerUpgradeIcon() {
        _towerUpgradeMenu.EnableTowerUpgradeIcon(true);
    }

    public void DisableTowerUpgradeIcon() {
        _towerUpgradeMenu.EnableTowerUpgradeIcon(false);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag(Tags.bullet)) {
            Destroy(collision.gameObject);
        }
    }
}
