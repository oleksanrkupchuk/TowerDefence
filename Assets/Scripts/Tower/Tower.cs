﻿using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

    [SerializeField]
    private List<Enemy> _enemyList = new List<Enemy>();
    public List<Enemy> EnemyList { get => _enemyList; }

    [Header("Parametrs")]
    [SerializeField]
    private float _rangeAttack;
    public float RangeAttack { get => _rangeAttack; }
    [SerializeField]
    private float _timeShoot;
    [SerializeField]
    private float _timer;
    [SerializeField]
    private Transform _bulletPosition;
    [SerializeField]
    private int _stepDegree;
    [SerializeField]
    private int _price;
    public int Price { get => _price; }

    [Header("Components visible")]
    [SerializeField]
    private GameObject _canvas;
    [SerializeField]
    private CircleCollider2D _rangeCollider;
    [SerializeField]
    private GameObject _buletPrefab;
    [SerializeField]
    private Bullet _buletScript;
    public Bullet BulletScript { get => _buletPrefab.GetComponent<Bullet>(); }
    [SerializeField]
    private LineRenderer _lineRenderer;

    [Header("Components invisible")]
    [SerializeField]
    private Enemy target = null;
    private TowerManager _towerManager;
    private GameManager _gameManager;
    private Collider2D[] _towerAllColliders;
    [SerializeField]
    private GameObject _placeForTower = null;

    private bool _isShooting = false;

    [Header("Upgrades Tower")]
    [SerializeField]
    private GameObject _objectIncreaseDamage;
    [SerializeField]
    private GameObject _objectSell;
    [SerializeField]
    private GameObject _objectIncreaseRange;
    [SerializeField]
    private TowerUpgradeMenu _towerUpgradeMenu;
    [SerializeField]
    private TowerRange _towerRange;

    int countBullet = 0;

    public void Initialization(TowerManager towerManager, GameManager gameManager) {
        _towerManager = towerManager;
        _gameManager = gameManager;
        _towerUpgradeMenu.Initialization(_gameManager, this);
    }

    private void Start() {
        _lineRenderer.enabled = false;
        DisableCanvas();
        SetRadiusInLineRanderer(_rangeAttack);
        _towerManager.towersList.Add(this);
        //SetPositionUpgradeIcon(2f);

        GetAllColliderOnTower();

        _buletScript.SetBasicDamage();

        //_towerUpgradeMenu.SubscribleButtonOnEvent();
    }

    private void SetRadiusInLineRanderer(float radius) {
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
        _towerAllColliders = GetComponents<Collider2D>();
    }

    void Update() {
        if (_isShooting) {
            _timer -= Time.deltaTime;
        }

        if (_timer <= 0f) {
            if (target != null && _enemyList.Count > 0) {
                Shoot(target);
                _timer = _timeShoot;
                _isShooting = true;
            }
        }
    }

    public void SetTarget() {
        if (_enemyList.Count > 0) {
            target = _enemyList[0];
            //print("tower = " + gameObject.name);
            //print("target = " + target.name);
        }
        else {
            target = null;
            //Debug.LogWarning("list of enemys empty");
        }
    }

    private void Shoot(Enemy target) {
        GameObject _bulletObject = Instantiate(_buletPrefab, _bulletPosition.position, transform.rotation);
        _bulletObject.name = "bullet " + countBullet;
        countBullet++;
        Collider2D _bulletCollider = _bulletObject.GetComponent<CircleCollider2D>();
        IgnoreCollisionCollidersTowerAndBullet(_bulletCollider);
        Bullet _bulletScr = _bulletObject.GetComponent<Bullet>();
        _bulletScr.SetTower(this);
        _bulletScr.SetTarget(target);
    }

    private void IgnoreCollisionCollidersTowerAndBullet(Collider2D bulletCollider) {
        for (int count = 0; count < _towerAllColliders.Length; count++) {
            Physics2D.IgnoreCollision(_towerAllColliders[count], bulletCollider, true);
        }
    }

    public void IncreaseDamage() {
        _buletScript.Damage += Damage();
        //print("tower damage = " + _buletScript.Damage);
    }

    private int Damage() {
        int damage = 0;
        damage += _buletScript.Damage / 2;
        if (damage < 1) {
            damage = 1;
        }
        return damage;
    }

    public void IncreaseRange(float range) {
        //print("Range up");
        _rangeAttack += range;
        _rangeCollider.radius = _rangeAttack;
        SetRadiusInLineRanderer(_rangeAttack);
    }

    public void EnableLineRenderer() {
        _lineRenderer.enabled = true;
    }

    public void DisableLineRenderer() {
        _lineRenderer.enabled = false;
    }

    public bool IsActiveCanvas() {
        if (_canvas.activeSelf == true) {
            return true;
        }

        return false;
    }

    public void EnableTowerUpgradeIcon() {
        _towerUpgradeMenu.EnableTowerUpgradeIcon(true);
    }

    public void EnableCanvas() {
        _canvas.gameObject.SetActive(true);
    }

    public void DisableCanvas() {
        _canvas.gameObject.SetActive(false);
    }

    public void RemoveTarget(Enemy enemy) {
        _enemyList.Remove(enemy);
        //for (int i = 0; i < _enemyList.Count; i++) {
        //    print($"enemy {i} name = " + _enemyList[i].name);
        //}
    }

    public bool IsTargetNull() {
        if (target == null) {
            return true;
        }

        return false;
    }

    public void SetPlaceForTower(GameObject placeForTower) {
        _placeForTower = placeForTower;
    }

    public void EnableColliderOnPlaceForTower() {
        _placeForTower.GetComponent<Collider2D>().enabled = true;
    }

    public void DisableColliderOnPlaceForTower() {
        _placeForTower.GetComponent<Collider2D>().enabled = false;
    }

    public void RemoveTowerFromList() {
        _towerManager.towersList.Remove(this);
    }

    public void DestroyTower(float timeDestroy) {
        Destroy(gameObject, timeDestroy);
    }
}
