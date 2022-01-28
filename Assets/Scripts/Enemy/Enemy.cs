using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public abstract class Enemy : MonoBehaviour {
    protected GameManager _gameManager;
    protected EnemySpawner _enemySpawner;
    protected Transform _nextWayPoint;
    protected int _indexPosition;
    protected List<Transform> _currentWay = new List<Transform>();
    protected Tower _tower;
    protected AnimationEvent _enemyAnimationDead = new AnimationEvent();
    protected string _isDead = "isDying";

    [Header("Parametrs")]
    [SerializeField]
    protected int _speed;
    [SerializeField]
    protected int _health;
    [SerializeField]
    protected int _healthMax;
    [SerializeField]
    protected int _amountCoinForDeath;
    [SerializeField]
    protected int _sortingLayer;

    [Header("Components")]
    [SerializeField]
    protected BoxCollider2D _boxCollider;
    [SerializeField]
    protected Animator _animator;
    [SerializeField]
    protected SpriteRenderer _spriteRenderer;
    [SerializeField]
    protected Canvas _canvas;

    [Header("GameObjects")]
    [SerializeField]
    protected GameObject _healthBarBackground;

    [Header("UI")]
    [SerializeField]
    protected Image _healthBar;

    [Header("Animator parametrs")]
    [SerializeField]
    protected AnimationClip _deadAnimationClip;

    [HideInInspector]
    public Vector3 lastPosition;
    public bool IsDead { get => _health <= 0; }
    public event Action LastPosition;

    public void Initialization(GameManager gameManager, EnemySpawner enemySpawner) {
        _gameManager = gameManager;
        _enemySpawner = enemySpawner;
    }

    public void InitializationTower(Tower tower) {
        _tower = tower;
    }

    protected void Start() {
        if (_boxCollider == null) {
            Debug.LogError("component is null");
        }

        _healthBarBackground.SetActive(true);

        SubscribeToEventInTheEndDeadAnimation();
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
        transform.position = Vector2.MoveTowards(transform.position, _nextWayPoint.position, _speed * Time.deltaTime);
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

    public void TakeDamage(int damage) {
        //print("--------------");
        //print("HIT = " + damage);

        if (!IsDead) {
            _health -= damage;
            ShiftHealthBar();
            //GetLastPosition();

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
        _gameManager.AddCoin(_amountCoinForDeath);
        _enemySpawner.RemoveEnemy(this);
        DisableHealthBar();
        _boxCollider.enabled = false;
        _tower.RemoveTarget(this);
        _tower.SetTarget();
        _gameManager.CheckLastEnemyAndEnableWinMenu();
        PlayDeadAnimation();
    }

    protected void SubscribeToEventInTheEndDeadAnimation() {
        float _playingAnimationTime = _deadAnimationClip.length;
        _enemyAnimationDead.time = _playingAnimationTime;
        _enemyAnimationDead.functionName = nameof(DestroyEnemy);

        _deadAnimationClip.AddEvent(_enemyAnimationDead);
    }

    public void GetLastPosition() {
        lastPosition = transform.position;
        //print("last position = " + diePosition);
        LastPosition?.Invoke();
    }

    protected void DestroyEnemy() {
        Destroy(gameObject, 0.5f);
    }

    protected void PlayDeadAnimation() {
        _animator.SetTrigger(_isDead);
    }

    protected void ShiftHealthBar() {
        _healthBar.fillAmount = (float)_health / _healthMax;
    }

    protected void DisableHealthBar() {
        _healthBarBackground.SetActive(false);
    }

    public void SetWayPoints(List<Transform> currentWay) {
        _currentWay = currentWay;
        _nextWayPoint = _currentWay[0];
    }
}
