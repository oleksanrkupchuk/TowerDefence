using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public abstract class Enemy : MonoBehaviour {
    protected GameManager _gameManager;
    protected EnemySpawner _enemySpawner;
    protected Transform _nextWayPoint;
    protected int _indexPosition;
    protected Tower _tower;
    protected string _isDead = "isDying";
    protected float _speed;
    protected List<Transform> _currentWay = new List<Transform>();
    protected AnimationEvent _enemyEventDead = new AnimationEvent();

    [Header("Parametrs")]
    [SerializeField]
    protected float _defaultSpeed;
    [SerializeField]
    protected float _health;
    [SerializeField]
    protected float _healthMax;
    [SerializeField]
    protected int _amountCoinForDeath;
    [SerializeField]
    protected int _sortingLayer;

    [Header("Components")]
    [SerializeField]
    protected Collider2D _collider;
    [SerializeField]
    protected Animator _animator;
    [SerializeField]
    protected SpriteRenderer _spriteRenderer;
    [SerializeField]
    protected Canvas _canvas;
    [SerializeField]
    protected EnemyDebuff _debuff;
    [SerializeField]
    protected ParticleSystem _burningEffect;

    [Header("GameObjects")]
    [SerializeField]
    protected GameObject _healthBarBackground;

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

    [HideInInspector]
    public Vector3 lastPosition;
    public bool IsDead { get => _health <= 0; }
    public event Action LastPosition;
    public Animator Animator { get => _animator; }
    public EnemyDebuff Debuff { get => _debuff; }

    public void Init(GameManager gameManager, EnemySpawner enemySpawner) {
        _gameManager = gameManager;
        _enemySpawner = enemySpawner;
    }

    public void InitializationTower(Tower tower) {
        _tower = tower;
    }

    protected void Start() {
        InitSpeed();
        StopBurningEffect();

        if (_collider == null) {
            Debug.LogError("component is null");
        }

        _healthBarBackground.SetActive(true);
    }

    public void InitSpeed() {
        _speed = _defaultSpeed;
    }

    public void AddDestroyEventForDeadAnimation() {
        float _playingAnimationTime = _deadClip.length;
        _enemyEventDead.time = _playingAnimationTime;
        _enemyEventDead.functionName = nameof(DestroyEnemy);

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

    protected void Move() {
        transform.position = Vector2.MoveTowards(transform.position, _nextWayPoint.position, _defaultSpeed * Time.deltaTime);
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

    public void TakeDamage(float damage) {
        if (!IsDead) {
            _health -= damage;
            ShiftHealthBar();

            //print("enemy take damage = " + damage);

            if (IsDead) {
                DeathFromBullet();
            }
        }
    }

    public void DeathFromLastWay() {
        _enemySpawner.RemoveEnemy(this);
        _gameManager.TakeAwayOneHealth();
        _gameManager.CheckHealthAndShowLoseMenuIfHealthZero();
        DestroyEnemy();
    }

    protected void DeathFromBullet() {
        GetLastPosition();
        _tower.SetPositionTarget(transform);
        _gameManager.AddCoin(_amountCoinForDeath);
        _enemySpawner.RemoveEnemy(this);
        DisableHealthBar();
        _tower.RemoveTarget(this);
        _tower.SetTarget();
        _gameManager.CheckLastEnemyAndEnableWinMenu();
        PlayDeadAnimation();
    }

    public void GetLastPosition() {
        lastPosition = transform.position;
        LastPosition?.Invoke();
    }

    protected void DestroyEnemy() {
        Destroy(gameObject, 0.5f);
    }

    protected void PlayDeadAnimation() {
        _animator.SetTrigger(_isDead);
    }

    protected void ShiftHealthBar() {
        _healthBar.fillAmount = _health / _healthMax;
    }

    protected void DisableHealthBar() {
        _healthBarBackground.SetActive(false);
    }

    public void SetWayPoints(List<Transform> currentWay) {
        _currentWay = currentWay;
        _nextWayPoint = _currentWay[0];
    }

    public void SetSpeed(float value) {
        _speed = value;
    }

    public void PlayBurningEffect() {
        _burningEffect.Play();
    }

    public void StopBurningEffect() {
        _burningEffect.Stop();
    }
}
