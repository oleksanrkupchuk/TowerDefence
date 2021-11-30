using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour {
    [Header("Parametrs")]
    [SerializeField]
    private int _speed;
    [SerializeField]
    private int _health;
    [SerializeField]
    private int _healthMax;
    public bool isDead { get => _health <= 0; }
    [SerializeField]
    private int _coinForDeath;
    [SerializeField]
    private int _sortingLayer;
    public Vector3 diePosition;

    public float CenterBoxColliderOnY { get => (_boxCollider.bounds.size.y / 2); }

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

    private EnemySpawner _enemySpawner;
    private GameManager _gameManager;

    [Header("Animator parametrs")]
    private string _isDying = "isDying";

    private Transform _nextWayPoint;
    [SerializeField]
    private int _index;

    private List<Transform> _currentWayPoint = new List<Transform>();
    private List<DataWayPoints> _dataWayPoints = new List<DataWayPoints>();
    private Tower _tower;

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
    }

    private void Update() {
        if (!isDead) {
            SetNextPosition();

            Move();
        }
    }

    private void Move() {
        transform.position = Vector2.MoveTowards(transform.position, _nextWayPoint.position, _speed * Time.deltaTime);
    }

    private void SetNextPosition() {
        if (Vector2.Distance(transform.position, _nextWayPoint.position) <= 0.2f) {
            _index++;
            if (_index < _currentWayPoint.Count) {
                _nextWayPoint = _currentWayPoint[_index];
            }
        }
    }

    public void TakeDamage(int damage) {
        //print("--------------");
        //print("HIT = " + damage);

        if (!isDead) {
            _health -= damage;
            ShiftHealthBar();

            if (isDead) {
                Death();
            }
        }
    }

    private void ShiftHealthBar() {
        _healthBar.fillAmount = (float)_health / _healthMax;
    }

    private void Death() {
        diePosition = transform.position;
        _gameManager.AddCoin(_coinForDeath);
        _enemySpawner.RemoveEnemy(gameObject.GetComponent<Enemy>());
        PlayDeathAnimation();
        DisableHealthBar();
        //gameObject.SetActive(false);
        _boxCollider.enabled = false;
        _tower.RemoveTarget(this);
        _tower.SetTarget();
        _gameManager.EnableTimerWaveAndSetValueForTimer();
    }

    private void PlayDeathAnimation() {
        _animator.SetTrigger(_isDying);
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
            print("random number = " + randomWayPoints);
            _currentWayPoint = _dataWayPoints[randomWayPoints].WayPoints;
            _nextWayPoint = _currentWayPoint[0];
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag(Tags.finish)) {
            Death();
        }
    }

    public void SetLayer(int layer) {
        _spriteRenderer.sortingOrder = layer;
        _canvas.sortingOrder = layer;
    }
}
