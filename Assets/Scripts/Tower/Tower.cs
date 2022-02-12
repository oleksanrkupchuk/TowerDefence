using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour {
    protected AnimationEvent _towerEventShoot = new AnimationEvent();
    protected AnimationEvent _towerEventResetShoot = new AnimationEvent();
    protected bool _isShooting = false;
    protected int countBullet = 0;
    protected int _damage;
    protected TowerManager _towerManager;
    protected GameManager _gameManager;
    protected Transform _targetPosition;

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
    [SerializeField]
    protected Animator _animator;
    [SerializeField]
    protected AnimationClip _shootAnimation;
    [SerializeField]
    protected int _shootFrameRateInShootAnimation;

    [Header("Components visible")]
    [SerializeField]
    protected CircleCollider2D _rangeCollider;
    [SerializeField]
    protected Bullet _bullet;
    [SerializeField]
    protected LineRenderer _lineRenderer;

    [Header("Components invisible")]
    [SerializeField]
    protected Enemy target = null;
    [SerializeField]
    protected PlaceForTower _placeForTower = null;

    [Header("Upgrades Tower")]
    [SerializeField]
    protected TowerUpgradeMenu _towerUpgradeMenu;

    public int Price { get => _price; }
    public float RangeAttack { get => _rangeAttack; }
    public List<Enemy> EnemyList { get => _enemyList; }
    public int Damage { get => _damage; }
    public PlaceForTower PlaceForTower { get => _placeForTower; }
    public TowerUpgradeMenu TowerUpgradeMenu { get => _towerUpgradeMenu; }

    public void Initialization(TowerManager towerManager, GameManager gameManager) {
        _towerManager = towerManager;
        _gameManager = gameManager;
        _towerUpgradeMenu.Initialization(_gameManager, this);
    }

    protected void Start() {
        _lineRenderer.enabled = false;
        DisableUpgradeMenu();
        SetRangeRadius();
        _towerManager.towersList.Add(this);

        _damage = _bullet.Damage;
    }

    public void ReducePrice(int percent) {
        int _priceInterest = (_price * percent) / 100;
        //print("_priceInterest = " + _priceInterest);
        _price -= _priceInterest;
        //print("price = " + _price);
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
            if (target != null) { //&& _enemyList.Count > 0
                _isShooting = false;
                _timer = _timeShoot;
                PlayShootAnimation();
            }
        }
    }

    protected void PlayShootAnimation() {
        //print("play Animation shoot");
        _animator.SetTrigger("shoot");
    }

    public void AddShootEventForShootAnimation() {
        float _playingAnimationTime = _shootFrameRateInShootAnimation / _shootAnimation.frameRate;
        _towerEventShoot.time = _playingAnimationTime;
        _towerEventShoot.functionName = nameof(Shoot);

        _shootAnimation.AddEvent(_towerEventShoot);
    }

    protected void Shoot() {
        //print("distance to target = " + _distanceToTarget);
        //print("shoot tower " + gameObject.name);
        Bullet _bulletObject = Instantiate(_bullet, _bulletPosition.position, _bullet.transform.rotation);
        _bulletObject.SetDamage(_damage);
        if (target == null) {
            float _distanceToTarget = Vector2.Distance(transform.position, _targetPosition.position);
            _bulletObject.Init(this, _targetPosition);
            _bulletObject.SetDistanceAndRange(_distanceToTarget, _rangeAttack);
        }
        else {
            float _distanceToTarget = Vector2.Distance(transform.position, target.transform.position);
            _bulletObject.Init(this);
            _bulletObject.SetDistanceAndRange(_distanceToTarget, _rangeAttack);
        }
        _bulletObject.name = "bullet " + countBullet;
        countBullet++;
    }

    public void AddResetShootEventForShootAnimation() {
        float _playingAnimationTime = _shootAnimation.length;
        _towerEventResetShoot.time = _playingAnimationTime - 0.01f;
        _towerEventResetShoot.functionName = nameof(ResetShoot);

        _shootAnimation.AddEvent(_towerEventResetShoot);
    }

    protected void ResetShoot() {
        //_isShooting = true;
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

    public bool IsActiveUpgradeMenu() {
        if (_towerUpgradeMenu.gameObject.activeSelf == true) {
            return true;
        }

        return false;
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

    public void EnableUpgradeMenu() {
        _towerUpgradeMenu.gameObject.SetActive(true);
    }

    public void DisableUpgradeMenu() {
        _towerUpgradeMenu.gameObject.SetActive(false);
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

    public Enemy GetTarget() {
        if (target != null) {
            return target;
        }

        return null;
    }

    public void SetPositionTarget(Transform targetPosition) {
        _targetPosition = targetPosition;
    }

    public void SetPlaceForTower(PlaceForTower placeForTower) {
        _placeForTower = placeForTower;
    }

    public void RemoveTowerFromList() {
        _towerManager.towersList.Remove(this);
    }

    public void DestroyTower(float timeDestroy) {
        Destroy(gameObject, timeDestroy);
    }
}
