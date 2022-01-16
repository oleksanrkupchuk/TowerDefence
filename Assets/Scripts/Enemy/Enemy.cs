using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {
    private GameManager _gameManager;
    private Transform _nextWayPoint;
    private int _indexPosition;
    private List<Transform> _currentWay = new List<Transform>();
    private Tower _tower;
    private AnimationEvent _enemyAnimationDead = new AnimationEvent();
    private string _isDead = "isDying";

    [Header("Parametrs")]
    [SerializeField]
    private int _speed;
    [SerializeField]
    private int _health;
    [SerializeField]
    private int _healthMax;
    [SerializeField]
    private int _amountCoinForDeath;
    [SerializeField]
    private int _sortingLayer;

    [Header("Components")]
    [SerializeField]
    private BoxCollider2D _boxCollider;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private Canvas _canvas;

    [Header("GameObjects")]
    [SerializeField]
    private GameObject _healthBarBackground;

    [Header("UI")]
    [SerializeField]
    private RectTransform _healthBarRectTransform;
    [SerializeField]
    private Image _healthBar;

    [Header("Animator parametrs")]
    [SerializeField]
    private AnimationClip _deadAnimationClip;

    [SerializeField]
    private bool _isCloseForWayPoint;

    public bool IsDead { get => _health <= 0; }
    public Vector3 lastPosition;
    public event Action LastPosition;
    public static event Action<Enemy> EnemyDead;

    public void Initialization(GameManager gameManager) {
        _gameManager = gameManager;
    }

    public void InitializationTower(Tower tower) {
        _tower = tower;
    }

    private void Start() {
        if (_boxCollider == null) {
            Debug.LogError("component is null");
        }

        _healthBarBackground.SetActive(true);

        //_enemySpawner.AddEnemy(gameObject.GetComponent<Enemy>());
        SubscribeToEventInTheEndDeadAnimation();
    }

    private void Update() {
        if (!IsDead) {

            if (_currentWay != null) {
                SetNextPosition();
                Move();
            }
        }
    }

    private void Move() {
        transform.position = Vector2.MoveTowards(transform.position, _nextWayPoint.position, _speed * Time.deltaTime);
    }

    private void SetNextPosition() {
        if (Vector2.Distance(transform.position, _nextWayPoint.position) <= 0.2f) {
            _indexPosition++;
            if (_indexPosition < _currentWay.Count) {
                _nextWayPoint = _currentWay[_indexPosition];
            }

            CheckFlipSprite();
        }
    }

    private void CheckFlipSprite() {
        if (transform.position.x - _nextWayPoint.position.x < 0) {
            FlipSpriteLeft();
        }

        if (transform.position.x - _nextWayPoint.position.x > 0) {
            FlipSpriteRight();
        }
    }

    private void FlipSpriteLeft() {
        _spriteRenderer.flipX = false;
    }

    private void FlipSpriteRight() {
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
        EnemyDead?.Invoke(this);
        _gameManager.TakeAwayOneHealth();
        _gameManager.CheckHealthAndShowLoseMenuIfHealthZero();
        DestroyEnemy();
    }

    private void DeathFromBullet() {
        GetLastPosition();
        _gameManager.AddCoin(_amountCoinForDeath);
        EnemyDead?.Invoke(this);
        DisableHealthBar();
        _boxCollider.enabled = false;
        _tower.RemoveTarget(this);
        _tower.SetTarget();
        _gameManager.CheckLastEnemyAndEnableWinMenu();
        PlayDeadAnimation();
    }

    private void SubscribeToEventInTheEndDeadAnimation() {
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

    private void DestroyEnemy() {
        Destroy(gameObject, 0.5f);
    }

    private void PlayDeadAnimation() {
        _animator.SetTrigger(_isDead);
    }

    private void ShiftHealthBar() {
        _healthBar.fillAmount = (float)_health / _healthMax;
    }

    private void DisableHealthBar() {
        _healthBarBackground.SetActive(false);
    }

    public void SetWayPoints(List<Transform> currentWay) {
        _currentWay = currentWay;
        _nextWayPoint = _currentWay[0];
    }
}
