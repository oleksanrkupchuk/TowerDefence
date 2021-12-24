using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour {
    private EnemySpawner _enemySpawner;
    private GameManager _gameManager;
    private Transform _nextWayPoint;
    private int _indexPosition;
    private List<Transform> _currentWayPoint = new List<Transform>();
    private List<DataWayPoints> _dataWayPoints = new List<DataWayPoints>();
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
    public Vector3 diePosition;
    public event Action Dead;
    public event Action OutRangeTower;

    public void Initialization(EnemySpawner enemySpawner, GameManager gameManager, List<DataWayPoints> dataWayPoints) {
        _enemySpawner = enemySpawner;
        _gameManager = gameManager;
        _dataWayPoints = dataWayPoints;
    }

    public void InitializationTower(Tower tower) {
        _tower = tower;
    }

    private void Start() {
        if (_boxCollider == null) {
            Debug.LogError("component is null");
        }

        _healthBarBackground.SetActive(true);

        _enemySpawner.AddEnemy(gameObject.GetComponent<Enemy>());
        SetWayPoints();
        SubscribeToEventInTheEndDeadAnimation();
    }

    private void Update() {
        if (!IsDead) {
            SetNextPosition();

            //ChangeMaxLayer();

            Move();

            CkeckLastWayPoint();
        }
    }

    private void Move() {
        transform.position = Vector2.MoveTowards(transform.position, _nextWayPoint.position, _speed * Time.deltaTime);
    }

    private void SetNextPosition() {
        if (Vector2.Distance(transform.position, _nextWayPoint.position) <= 0.2f) {
            _indexPosition++;
            if (_indexPosition < _currentWayPoint.Count) {
                _nextWayPoint = _currentWayPoint[_indexPosition];
            }
            _isCloseForWayPoint = true;

            CheckFlipSprite();
        }
    }

    private void CkeckLastWayPoint() {
        if (IsCloseForLasrWayPoint()) {
            DeathFromLastWay();
        }
    }

    private bool IsCloseForLasrWayPoint() {
        int _lastPoint = _currentWayPoint.Count - 1;
        if (_nextWayPoint == _currentWayPoint[_lastPoint]) {
            if (Vector2.Distance(transform.position, _nextWayPoint.position) <= 0.2f) {
                return true;
            }
        }

        return false;
    }

    private void CheckFlipSprite() {
        if (transform.position.x - _nextWayPoint.position.x < 0) {
            FlipSpriteLeft();
        }

        if (transform.position.x - _nextWayPoint.position.x > 0) {
            FlipSpriteRight();
        }
    }

    public void ChangeMaxLayer() {
        float _distanceForWayPoint = Math.Abs(transform.position.y - _nextWayPoint.position.y);
        //print("Distance = " + _distanceForWayPoint);
        if (_distanceForWayPoint < 1f && _isCloseForWayPoint) {
            _isCloseForWayPoint = false;
            int index = _indexPosition + 1;
            Transform nextWayPoint = _currentWayPoint[index];
            if (transform.position.y - nextWayPoint.position.y < 0) {
                ChangeLayerOnMin();
            }

            if (transform.position.y - nextWayPoint.position.y > 0) {
                ChangeLayerOnMax();

            }
        }
    }

    private void ChangeLayerOnMax() {
        SetLayer(_enemySpawner.maxLayerEnemy);
        _enemySpawner.maxLayerEnemy--;
    }

    private void ChangeLayerOnMin() {
        SetLayer(_enemySpawner.minLayerEnemy);
        _enemySpawner.minLayerEnemy++;
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

            if (IsDead) {
                DeathFromBullet();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag(Tags.finish)) {
            DeathFromBullet();
        }
    }

    private void DeathFromLastWay() {
        _enemySpawner.RemoveEnemy(this);
        _gameManager.TakeAwayOneHealth();
        _gameManager.CheckHealthAndShowLoseMenuIfHealthZero();
        DestroyEnemy();
    }

    private void DeathFromBullet() {
        GetDiePosition();
        _gameManager.AddCoin(_amountCoinForDeath);
        _enemySpawner.RemoveEnemy(this);
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

    public void GetDiePosition() {
        diePosition = transform.position;
        //print("die position = " + diePosition);
        Dead?.Invoke();
    }

    public void GetLastPosition() {
        diePosition = transform.position;
        //print("last position = " + diePosition);
        OutRangeTower?.Invoke();
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

    public void SetWayPoints() {
        if (_dataWayPoints.Count <= 1) {
            _currentWayPoint = _dataWayPoints[0].WayPoints;
            _nextWayPoint = _currentWayPoint[0];
        }
        else {
            int randomWayPoints = Random.Range(0, _dataWayPoints.Count);
            //print("random number = " + randomWayPoints);
            _currentWayPoint = _dataWayPoints[randomWayPoints].WayPoints;
            _nextWayPoint = _currentWayPoint[0];
        }
    }

    public void SetLayer(int layer) {
        _spriteRenderer.sortingOrder = layer;
        _canvas.sortingOrder = layer;
    }
}
