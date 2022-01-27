using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour {

    protected bool _isShooting = false;
    protected int countBullet = 0;
    protected int _damage;
    protected TowerManager _towerManager;
    protected GameManager _gameManager;

    [SerializeField]
    protected List<Enemy> _enemyList = new List<Enemy>();

    [Header("Parametrs")]
    [SerializeField]
    protected float _rangeAttack;
    [SerializeField]
    protected float _timeShoot;
    [SerializeField]
    protected float _timer;
    [SerializeField]
    protected Transform _bulletPosition;
    [SerializeField]
    protected int _stepDegree;
    [SerializeField]
    protected int _price;

    [Header("Components visible")]
    [SerializeField]
    protected GameObject _canvas;
    [SerializeField]
    protected CircleCollider2D _rangeCollider;
    [SerializeField]
    protected GameObject _buletPrefab;
    [SerializeField]
    protected Bullet _buletScript;
    [SerializeField]
    protected LineRenderer _lineRenderer;

    [Header("Components invisible")]
    [SerializeField]
    protected Enemy target = null;
    [SerializeField]
    protected GameObject _placeForTower = null;

    [Header("Upgrades Tower")]
    [SerializeField]
    protected TowerUpgradeMenu _towerUpgradeMenu;

    public int Price { get => _price; }
    public float RangeAttack { get => _rangeAttack; }
    public List<Enemy> EnemyList { get => _enemyList; }
    public int Damage { get => _damage; }

    public void Initialization(TowerManager towerManager, GameManager gameManager) {
        _towerManager = towerManager;
        _gameManager = gameManager;
        _towerUpgradeMenu.Initialization(_gameManager, this);
    }

    protected void Start() {
        _lineRenderer.enabled = false;
        DisableCanvas();
        SetRangeRadius();
        _towerManager.towersList.Add(this);

        _damage = _buletScript.Damage;
    }

    protected void SetRangeRadius() {
        _rangeCollider.radius = _rangeAttack;

        SetRadiusInLineRanderer(_rangeAttack);
    }

    protected void SetRadiusInLineRanderer(float radius) {
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

    protected void Update() {
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

    protected void Shoot(Enemy target) {
        GameObject _bulletObject = Instantiate(_buletPrefab, _bulletPosition.position, transform.rotation);
        _bulletObject.name = "bullet " + countBullet;
        countBullet++;
        Bullet _bulletScr = _bulletObject.GetComponent<Bullet>();
        //print("shoot damage = " + _damage);
        _bulletScr.SetDamage(_damage);
        _bulletScr.SetTower(this);
        _bulletScr.SetTarget(target);
    }

    public void IncreaseDamage() {
        _damage += DamageCalculation();
        //print("tower damage = " + _buletScript.Damage);
    }

    protected int DamageCalculation() {
        int damage = 0;
        damage += _damage / 2;
        if (damage < 1) {
            damage = 1;
        }
        //print("calculation damage = " + damage);
        return damage;
    }

    public void IncreaseRange(float range) {
        //print("Range up");
        _rangeAttack += range;
        _rangeCollider.radius = _rangeAttack;
        SetRangeRadius();
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
