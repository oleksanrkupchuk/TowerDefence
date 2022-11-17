using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour {
    protected AnimationEvent _towerEventShoot = new AnimationEvent();
    protected bool _isShooting = false;
    protected int countBullet = 0;
    protected float _bulletDamage;
    protected TowerManager _towerManager;
    protected GameManager _gameManager;
    protected Transform _targetPosition;
    protected List<Bullet> _poolBullets = new List<Bullet>();
    protected Bullet _currentBullet;
    protected List<BulletAbility> _bulletsAbility = new List<BulletAbility>();
    protected float _timer = 0;
    protected List<Enemy> _enemyList = new List<Enemy>();
    protected Enemy target = null;
    protected PlaceForTower _placeForTower = null;
    protected List<AudioSource> _shoSounds = new List<AudioSource>();

    [Header("Parametrs")]
    [SerializeField]
    protected float _rangeAttack;
    [SerializeField]
    protected float _timeShoot;
    [SerializeField]
    protected int _stepDegree;
    [SerializeField]
    protected int _price;

    [Header("Components")]
    [SerializeField]
    protected CircleCollider2D _rangeCollider;
    [SerializeField]
    protected Bullet _bullet;
    [SerializeField]
    private AudioClip _soundShot;
    [SerializeField]
    private Transform startBulletPosition;
    [SerializeField]
    protected Animator _animator;
    [SerializeField]
    protected AnimationClip _shootAnimation;
    [SerializeField]
    protected int _shootFrameRateInShootAnimation;
    [SerializeField]
    private Transform _poolBulletObject;
    [SerializeField]
    private Transform _poolShotSoounds;
    [SerializeField]
    private AudioSource _prefabShotSound;

    [Header("Upgrades Tower")]
    [SerializeField]
    protected TowerUpgradeMenu _towerUpgradeMenu;
    [SerializeField]
    private BulletAbility _bulletAbility;

    public float increaseDamage;

    public int Price { get => _price; }
    public float RangeAttack { get => _rangeAttack; }
    public Enemy Target { get => target; }
    public List<Enemy> EnemyList { get => _enemyList; }
    public PlaceForTower PlaceForTower { get => _placeForTower; }
    public Transform StartBulletPosition { get => startBulletPosition; }
    public List<BulletAbility> BulletsAbility { get => _bulletsAbility; }

    public void Init(TowerManager towerManager, GameManager gameManager, Camera camera) {
        _towerManager = towerManager;
        _gameManager = gameManager;
        _towerUpgradeMenu.Init(_gameManager, this, camera);

        CreateBulletAbility();
        CreatePoolBulet();
        CreateAndRegisteredShotSounds();
        InitExteranalEffectVolume();
    }

    private void CreateBulletAbility() {
        for (int i = 0; i < 5; i++) {
            BulletAbility _bulletAbilityObject = Instantiate(_bulletAbility);
            _bulletAbilityObject.transform.SetParent(startBulletPosition);
            _bulletAbilityObject.gameObject.SetActive(false);
            _bulletsAbility.Add(_bulletAbilityObject);
        }
    }

    private void CreatePoolBulet() {
        for (int i = 0; i < 5; i++) {
            Bullet _bulletObject = Instantiate(_bullet, startBulletPosition.position, _bullet.transform.rotation);
            _bulletObject.Init(this);
            _bulletObject.CreatePointsForBeizerTrajectory(_poolBulletObject);
            _bulletObject.gameObject.SetActive(false);
            _bulletObject.transform.SetParent(_poolBulletObject);
            _poolBullets.Add(_bulletObject);
        }
    }

    private void CreateAndRegisteredShotSounds() {
        for (int i = 0; i < 5; i++) {
            AudioSource _shotSoundObject = Instantiate(_prefabShotSound);
            _shotSoundObject.name = "sound" + i;
            _shotSoundObject.transform.SetParent(_poolShotSoounds);
            _shotSoundObject.clip = _soundShot;
            SoundManager.Instance.ExternalSoundEffects.Add(_shotSoundObject);
            _shoSounds.Add(_shotSoundObject);
        }
    }

    private void InitExteranalEffectVolume() {
        SettingsData _settingsData = SaveSystemSettings.LoadSettings();
        SoundManager.Instance.InitExternalEffectVolume(_settingsData);
    }

    protected void Awake() {
        if (_animator == null) {
            Debug.LogError("Animator is null");
            return;
        }
        if (_soundShot == null) {
            Debug.LogError("AudioSource is null");
            return;
        }
    }

    protected void Start() {
        Enemy.Dead += TargetDead;
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

    private void PlayShootSound() {
        for (int i = 0; i < _shoSounds.Count; i++) {
            if (!_shoSounds[i].isPlaying) {
                print("sound = " + _shoSounds[i].name);
                _shoSounds[i].Play();
                return;
            }
        }
    }

    public void ResetShoot() {
        _isShooting = true;
    }

    public void IncreaseDamage() {
        increaseDamage = (_bulletDamage * 30) / 100;
        _bulletDamage += increaseDamage;
        print("damage bullet " + _bulletDamage);
    }

    public void IncreaseRange(float range) {
        _rangeAttack += range;
        _rangeCollider.radius = _rangeAttack;
        SetRangeRadius();
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
            if (bullet.gameObject.activeSelf == true) {
                bullet.EnemyOutRangeTower(enemy);
            }
        }
    }

    protected void OnDestroy() {
        Enemy.Dead -= TargetDead;
    }
}
