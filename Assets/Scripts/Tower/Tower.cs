using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour {
    protected AnimationEvent _towerEventShoot = new AnimationEvent();
    [SerializeField]
    protected bool _isShooting = false;
    protected int countBullet = 0;
    protected int _damage;
    protected TowerManager _towerManager;
    protected GameManager _gameManager;
    protected Transform _targetPosition;
    [SerializeField]
    protected List<Bullet> _bullets = new List<Bullet>();
    [SerializeField]
    protected List<Bullet> _poolBullets = new List<Bullet>();
    [SerializeField]
    protected Bullet _currentBullet;

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
    protected int _stepDegree;
    [SerializeField]
    protected int _price;


    public Transform _bulletPosition;
    [SerializeField]
    protected Animator _animator;
    [SerializeField]
    protected AnimationClip _shootAnimation;
    [SerializeField]
    protected int _shootFrameRateInShootAnimation;
    [SerializeField]
    private Transform _poolBullet;

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

    public int increaseDamage;

    public int Price { get => _price; }
    public Enemy Target { get => target; }
    public float RangeAttack { get => _rangeAttack; }
    public List<Enemy> EnemyList { get => _enemyList; }
    public int Damage { get => _damage; }
    public PlaceForTower PlaceForTower { get => _placeForTower; }

    public void Init(TowerManager towerManager, GameManager gameManager) {
        _towerManager = towerManager;
        _gameManager = gameManager;
        _towerUpgradeMenu.Initialization(_gameManager, this);

        CreatePoolBulet();
    }

    private void CreatePoolBulet() {
        for (int i = 0; i < 5; i++) {
            Bullet _bulletObject = Instantiate(_bullet, _bulletPosition.position, _bullet.transform.rotation);
            _bulletObject.Init(this);
            _bulletObject.CreatePointsForBeizerTrajectory(_poolBullet);
            _bulletObject.gameObject.SetActive(false);
            _bulletObject.transform.SetParent(_poolBullet);
            _poolBullets.Add(_bulletObject);
        }
    }

    protected void Start() {
        DisableLineRenderer();
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
            if (target != null) { 
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
        PlayShootSound();
        GetCurrentBullet();

        //_currentBullet.transform.position = _bulletPosition.position;
        _currentBullet.SetDamage(_damage);

        if (target == null) {
            _currentBullet.SetParametrs(_targetPosition);
            _currentBullet.SetDistanceAndRange(_rangeAttack);
        }
        else {
            _currentBullet.SetParametrs();
            _currentBullet.SetDistanceAndRange(_rangeAttack);
        }
        _bullets.Add(_currentBullet);
    }

    protected void GetCurrentBullet() {
        for (int i = 0; i < _poolBullets.Count; i++) {
            if(_poolBullets[i].gameObject.activeSelf == false) {
                _currentBullet = _poolBullets[i];
                _currentBullet.gameObject.SetActive(true);
                return;
            }
        }
    }

    protected virtual void PlayShootSound() {

    }

    public void ResetShoot() {
        _isShooting = true;
    }

    public void IncreaseDamage() {
        increaseDamage = DamageCalculation();
        _damage += increaseDamage;
    }

    protected int DamageCalculation() {
        int damage = 0;
        damage += _damage / 2;
        if (damage < 1) {
            damage = 1;
        }
        return damage;
    }

    public void IncreaseRange(float range) {
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
            SetNearestEnemy();
            //target = _enemyList[0];
        }
        else {
            target = null;
        }
    }

    private void SetNearestEnemy() {
        float distanceToClosestEnemy = Mathf.Infinity;
        foreach (Enemy currentEnemy in _enemyList) {
            float distanceToEnemy = (currentEnemy.transform.position - transform.position).sqrMagnitude;
            if (distanceToEnemy < distanceToClosestEnemy) {
                target = currentEnemy;
            }
        }
    }

    public void EnableUpgradeMenu() {
        _towerUpgradeMenu.gameObject.SetActive(true);
    }

    public void DisableUpgradeMenu() {
        _towerUpgradeMenu.DisableTextAmountObject();
        _towerUpgradeMenu.gameObject.SetActive(false);
    }

    public void RemoveTarget(Enemy enemy) {
        _enemyList.Remove(enemy);
    }

    public bool IsTargetNull() {
        if (target == null) {
            return true;
        }

        return false;
    }

    public void SetTargetPosition(Transform targetPosition) {
        _targetPosition = targetPosition;
    }

    public Transform GetTargetPosition() {
        return _targetPosition;
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

    public void RemoveBulletsAndSetTargetNull() {
        foreach (Bullet bullet in _bullets) {
            //bullet.SetNullTarget();
        }
    }

    public void RemoveBullet(Bullet bullet) {
        _bullets.Remove(bullet);
    }
}
