using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {
    [SerializeField]
    private GameObject _canvas;

    [SerializeField]
    private CircleCollider2D _rangeCollider;

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

    [Header("Components")]
    [SerializeField]
    private GameObject _buletPrefab;
    [SerializeField]
    private Bullet _buletScript;
    public Bullet Bullet { get => _buletPrefab.GetComponent<Bullet>(); }
    [SerializeField]
    private LineRenderer _lineRenderer;
    private GameObject _bulletObject;
    [HideInInspector]
    public GameObject placeForTower = null;

    //[HideInInspector]
    public Enemy target = null;
    private TowerManager _towerManager;

    private bool _isShooting = false;

    private Collider2D[] _towerAllColliders;

    [Header("Upgrades Tower")]
    [SerializeField]
    private GameObject _objectIncreaseDamage;
    [SerializeField]
    private GameObject _objectSell;
    [SerializeField]
    private GameObject _objectIncreaseRange;

    [SerializeField]
    private TowerUpgradeMenu _towerUpgradeMenu;

    public void Initialization(TowerManager towerManager) {
        _towerManager = towerManager;
    }

    private void Start() {
        _lineRenderer.enabled = false;
        SetRangeRadius(_rangeAttack);
        _towerManager.towersList.Add(GetComponent<Tower>());
        //SetPositionUpgradeIcon(2f);

        GetAllColliderOnTower();

        _buletScript.SetBasicDamage();

        _towerUpgradeMenu.SubscribleButtonOnEvent(this);
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
        _towerAllColliders = GetComponents<Collider2D>();
        for (int i = 0; i < _towerAllColliders.Length; i++) {
            print("name collider = " + _towerAllColliders[i].name);
        }
    }

    void Update() {
        if (_isShooting) {
            _timer -= Time.deltaTime;
        }

        if (_timer <= 0f) {
            if (target != null && _enemyList.Count > 0) {
                Shoot(target);
                _isShooting = false;
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
            //Debug.LogWarning("list of enemys empty");
        }
    }


    private void Shoot(Enemy target) {
        //float _canShoot = Vector2.Distance(transform.position, target.transform.position);
        //print("" + _rangeAttack + " --- " + _canShoot);
        //if (_rangeAttack >= _canShoot) {
        //}
        _bulletObject = Instantiate(_buletPrefab, _bulletPosition.position, Quaternion.identity);
        Collider2D _bulletCollider = _bulletObject.GetComponent<CircleCollider2D>();
        IgnoreCollisionCollidersTowerAndBullet(_bulletCollider);
        _bulletObject.GetComponent<Bullet>().SetTarget(target);
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

    public void EnableCanvas() {
        _canvas.gameObject.SetActive(true);
    }

    public void DisavbleCanvas() {
        _canvas.gameObject.SetActive(false);
    }

    public void RemoveTarget(Enemy enemy) {
        _enemyList.Remove(enemy);
    }
}
