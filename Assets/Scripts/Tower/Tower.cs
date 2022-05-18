using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour {
    protected AnimationEvent _towerEventShoot = new AnimationEvent();
    [SerializeField]
    protected bool _isShooting = false;
    protected int countBullet = 0;
    protected int _bulletDamage;
    protected TowerManager _towerManager;
    protected GameManager _gameManager;
    protected Transform _targetPosition;
    [SerializeField]
    protected List<Bullet> _poolBullets = new List<Bullet>();
    [SerializeField]
    protected Bullet _currentBullet;
    protected List<BulletAbility> _bulletsAbility = new List<BulletAbility>();
    protected float _timer = 0;

    [SerializeField]
    protected List<Enemy> _enemyList = new List<Enemy>();

    [Header("Parametrs")]
    [SerializeField]
    protected float _rangeAttack;
    [SerializeField]
    protected float _timeShoot;
    [SerializeField]
    protected int _stepDegree;
    [SerializeField]
    protected int _price;

    [SerializeField]
    private Transform _bulletPosition;
    [SerializeField]
    protected Animator _animator;
    [SerializeField]
    protected AnimationClip _shootAnimation;
    [SerializeField]
    protected int _shootFrameRateInShootAnimation;
    [SerializeField]
    private Transform _poolBulletObject;

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

    [SerializeField]
    private BulletAbility _bulletAbility;

    public int increaseDamage;

    public int Price { get => _price; }
    public float RangeAttack { get => _rangeAttack; }
    public Enemy Target { get => target; }
    public List<Enemy> EnemyList { get => _enemyList; }
    public PlaceForTower PlaceForTower { get => _placeForTower; }
    public Transform BulletPosition { get => _bulletPosition; }
    public List<BulletAbility> BulletsAbility { get => _bulletsAbility; }

    public void Init(TowerManager towerManager, GameManager gameManager) {
        _towerManager = towerManager;
        _gameManager = gameManager;
        _towerUpgradeMenu.Initialization(_gameManager, this);

        CreateBulletAbility();
        CreatePoolBulet();
    }

    private void CreateBulletAbility() {
        for (int i = 0; i < 5; i++) {
            BulletAbility _bulletAbilityObject = Instantiate(_bulletAbility);
            _bulletAbilityObject.transform.SetParent(_bulletPosition);
            _bulletAbilityObject.gameObject.SetActive(false);
            _bulletsAbility.Add(_bulletAbilityObject);
        }
    }

    private void CreatePoolBulet() {
        for (int i = 0; i < 5; i++) {
            Bullet _bulletObject = Instantiate(_bullet, _bulletPosition.position, _bullet.transform.rotation);
            _bulletObject.Init(this);
            _bulletObject.CreatePointsForBeizerTrajectory(_poolBulletObject);
            _bulletObject.gameObject.SetActive(false);
            _bulletObject.transform.SetParent(_poolBulletObject);
            _poolBullets.Add(_bulletObject);
        }
    }

    protected void Start() {
        Enemy.Dead += TargetDead;
        DisableLineRenderer();
        SetRangeRadius();
        _towerManager.towersList.Add(this);

        _bulletDamage = _bullet.Damage;
    }

    public void ReducePrice(int percent) {
        int _priceInterest = (_price * percent) / 100;
        //print("_priceInterest = " + _priceInterest);
        _price -= _priceInterest;
        //print("price = " + _price);
    }

    protected void SetRangeRadius() {
        _rangeCollider.radius = _rangeAttack;

        SetRadiusInLineRanderer(transform, _lineRenderer, _rangeAttack);
    }


    public void SetRadiusInLineRanderer(Transform transform, LineRenderer lineRenderer, float radius) {
        int _countStep = 360 / _stepDegree;

        lineRenderer.positionCount = _countStep;

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

        lineRenderer.SetPositions(_positionPoints);
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

        _currentBullet.SetDamage(_bulletDamage);

        if (target == null) {
            _currentBullet.SetDefaulPositionBulletAndTargetPosition(_targetPosition);
            _currentBullet.SetDistanceAndRange(_rangeAttack);
        }
        else {
            _currentBullet.SetDefaulPositionBulletAndTarget();
            _currentBullet.SetDistanceAndRange(_rangeAttack);
        }
    }

    protected void GetCurrentBullet() {
        for (int i = 0; i < _poolBullets.Count; i++) {
            if (_poolBullets[i].gameObject.activeSelf == false) {
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
        _bulletDamage += increaseDamage;
    }

    protected int DamageCalculation() {
        int damage = 0;
        damage += _bulletDamage / 2;
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

    public void SetNewTarget() {
        if (_enemyList.Count > 0) {
            SetNearestEnemy();
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

    public void TargetDead(Enemy enemy) {
        if (enemy == target) {
            SetTargetPosition(enemy.transform);
            RemoveTarget(enemy);
            SetNewTarget();
        }
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

    public void SetTargetPosition(Transform position) {
        _targetPosition = position;
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

    public void EnemyOutRange(Enemy enemy) {
        foreach (Bullet bullet in _poolBullets) {
            if(bullet.gameObject.activeSelf == true) {
                bullet.EnemyOutRangeTower(enemy);
            }
        }
    }

    protected void OnDestroy() {
        Enemy.Dead -= TargetDead;
    }
}
