using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(EnemyDebuff))]
[RequireComponent(typeof(Animator))]
public abstract class Enemy : MonoBehaviour {
    protected string _speedAnimatiorParametr = "speedStateWalking";
    protected bool _isIncreaseSpeed = false;
    protected float _durationIncreaseSpeed = 3.5f;

    protected SpriteRenderer _spriteRenderer;
    protected Rigidbody2D _rigidbody;
    protected EnemyDebuff _debuff;
    protected Animator _animator;
    protected CircleCollider2D _collider;
    protected GameManager _gameManager;
    protected EnemySpawner _enemySpawner;
    protected Transform _nextWayPoint;
    protected Camera _camera;
    protected int _indexPosition;
    protected Tower _tower;
    protected string _isDead = "isDying";
    [SerializeField]
    protected float _health;
    [SerializeField]
    protected float _speed;
    protected List<Transform> _currentWay = new List<Transform>();
    protected AnimationEvent _enemyEventDead = new AnimationEvent();

    [SerializeField]
    private CartSpawnInfoData _cartSpawnInfoData;

    [Header("Parametrs")]
    [SerializeField]
    protected float _defaultSpeed;
    [SerializeField]
    protected float _healthMax;
    [SerializeField]
    protected int _amountCoinForDeath;

    [Header("Components")]
    [SerializeField]
    protected Canvas _canvas;
    [SerializeField]
    protected EnemyCartData _enemyCartData;

    [Header("UI")]
    [SerializeField]
    protected Image _healthBar;

    [Header("Animator parametrs")]
    [SerializeField]
    protected AnimationClip _deadClip;
    [SerializeField]
    protected AnimationClip _walkingClip;
    [SerializeField]
    private AnimationState _animationState;

    [Header("Effects")]
    [SerializeField]
    protected ParticleSystem _healingEffect;
    [SerializeField]
    private ParticleSystem _burningEffect;

    [HideInInspector]
    public Vector3 lastPosition;

    public float Health { get => _health; }
    public float Speed { get => _defaultSpeed; }
    public bool IsDead { get => _health <= 0; }
    public event Action LastPosition;
    public Animator Animator { get => _animator; }
    public EnemyDebuff Debuff { get => _debuff; }
    public EnemyCartData EnemyCartData { get => _enemyCartData; }
    public CartSpawnInfoData CartSpawnInfoData { get => _cartSpawnInfoData; }

    public void Init(GameManager gameManager, EnemySpawner enemySpawner, Camera camera) {
        _gameManager = gameManager;
        _enemySpawner = enemySpawner;
        _camera = camera;

        GetComponents();

        _rigidbody.gravityScale = 0f;
        _collider.isTrigger = true;
        _collider.radius = 0.25f;
        _collider.offset = new Vector2(0f, 0.2f);
    }

    protected void GetComponents() {
        _collider = GetComponent<CircleCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _debuff = GetComponent<EnemyDebuff>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetTower(Tower tower) {
        _tower = tower;
    }

    protected void Start() {
        SetHealthToDefault();
        SetSpeedToDefault();
        StopBurningEffect();

        if (_collider == null) {
            Debug.LogError("component is null");
        }
    }

    public void SetHealthToDefault() {
        _health = _healthMax;
    }

    public void SetSpeedToDefault() {
        _speed = _defaultSpeed;
    }

    public void AddDestroyEventForDeadAnimation() {
        float _playingAnimationTime = _deadClip.length;
        _enemyEventDead.time = _playingAnimationTime;
        _enemyEventDead.functionName = nameof(DisableEnemyAfterTime);

        _deadClip.AddEvent(_enemyEventDead);
    }

    protected void Update() {
        if (!IsDead) {

            if (_currentWay != null) {
                SetNextPosition();
                Move();
            }
        }
    }

    protected void SetNextPosition() {
        if (Vector2.Distance(transform.position, _nextWayPoint.position) <= 0.2f) {
            _indexPosition++;
            if (_indexPosition < _currentWay.Count) {
                _nextWayPoint = _currentWay[_indexPosition];
            }

            CheckFlipSprite();
        }
    }

    protected void Move() {
        transform.position = Vector2.MoveTowards(transform.position, _nextWayPoint.position, _speed * Time.deltaTime);
    }

    protected void CheckFlipSprite() {
        if (transform.position.x - _nextWayPoint.position.x < 0) {
            FlipSpriteLeft();
        }

        if (transform.position.x - _nextWayPoint.position.x > 0) {
            FlipSpriteRight();
        }
    }

    protected void FlipSpriteLeft() {
        _spriteRenderer.flipX = false;
    }

    protected void FlipSpriteRight() {
        _spriteRenderer.flipX = true;
    }

    public virtual void TakeDamage(float damage) {
        if (!IsDead) {
            _health -= damage;
            SoundManager.Instance.PlaySoundEffect(SoundName.HitEnemy);
            ShiftHealthBar();

            if (IsDead) {
                DeathFromBullet();
            }
        }
    }

    protected void ShiftHealthBar() {
        _healthBar.fillAmount = _health / _healthMax;
    }

    protected void DeathFromBullet() {
        LastPosition?.Invoke();
        _tower.SetTargetPosition(transform);
        _tower.RemoveTarget(this);
        _tower.SetTarget();
        _gameManager.AddCoin(_amountCoinForDeath);
        _enemySpawner.RemoveEnemyInCurrentWave();
        DisableHealthBar();
        _gameManager.CheckLastEnemyAndEnableWinMenuOrSpawnNewEnemyWave();
        PlayDeadAnimation();
    }

    protected void DisableHealthBar() {
        _canvas.gameObject.SetActive(false);
    }

    protected void PlayDeadAnimation() {
        _animator.SetTrigger(_isDead);
    }

    public void DeathFromLastWay() {
        _enemySpawner.RemoveEnemyInCurrentWave();
        _gameManager.TakeAwayOneHealth();
        _gameManager.CheckHealthAndShowLoseMenuIfHealthZero();
        DisableEnemyAfterTime();
    }

    protected void DisableEnemyAfterTime() {
        Invoke(nameof(DisableEnemy), 0.5f);
    }

    protected void DisableEnemy() {
        gameObject.SetActive(false);
    }

    public void SetWayPoints(List<Transform> currentWay) {
        _currentWay = currentWay;
        _nextWayPoint = _currentWay[0];
    }

    public void SetSpeed(float value) {
        _speed = value;
    }

    public void Setlayer(int value) {
        _spriteRenderer.sortingOrder = value;
        SetLayerEffects(value);
    }

    public void AddHealth(float value) {
        _health += value;
        if (_health > _healthMax) {
            _health = _healthMax;
        }

        ShiftHealthBar();
    }

    public void PlayHealingEffect() {
        _healingEffect.Play();
    }

    public void PlayBurningEffect() {
        _burningEffect.Play();
    }

    public void StopBurningEffect() {
        _burningEffect.Stop();
    }

    public void SetLayerEffects(int value) {
        _burningEffect.GetComponent<ParticleSystemRenderer>().sortingOrder = value + 1;
        _healingEffect.GetComponent<ParticleSystemRenderer>().sortingOrder = value + 2;
    }

    public void SetSpeedAnimationWalking(float speed) {
        float _value = (speed * 1) / _defaultSpeed;
        _animator.SetFloat(_speedAnimatiorParametr, _value);
    }

    public IEnumerator IncreaseSpeed() {
        _isIncreaseSpeed = true;

        while (_isIncreaseSpeed) {
            _durationIncreaseSpeed -= Time.deltaTime;

            if (_durationIncreaseSpeed <= 0) {
                _isIncreaseSpeed = false;
            }

            yield return null;
        }

        SetSpeedToDefault();
        SetSpeedAnimationWalkingToDefault();

        _durationIncreaseSpeed = 3f;
    }

    public void SetSpeedAnimationWalkingToDefault() {
        _animator.SetFloat(_speedAnimatiorParametr, 1f);
    }
}
