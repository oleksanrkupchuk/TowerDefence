using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCMinotaur : BaseCharacterParametrs {
    private IState _currentState;
    private AnimationEvent _attackEvent = new AnimationEvent();
    private AnimationEvent _deadkEvent = new AnimationEvent();
    private List<Enemy> _enemies = new List<Enemy>();
    [SerializeField]
    private Enemy _target;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Gold _gold;
    private Vector2 _startPosition;

    [Header("Parametrs")]
    [SerializeField]
    private List<Transform> _wayPoints;
    [SerializeField]
    private BoxCollider2D _bodyCollider;
    [SerializeField]
    private BoxCollider2D _attackCollider;
    [SerializeField]
    private NPCMinotaurDialog _dialog;
    [SerializeField]
    private Image _healthBar;
    [SerializeField]
    private AnimationClip _attackClip;
    [SerializeField]
    private AnimationClip _deadClip;
    [SerializeField]
    private int _frameRateEnableAttackCollider;
    [SerializeField]
    private int _frameRateDisableAttackCollider;
    [SerializeField]
    private GameManager _gameManager;
    [SerializeField]
    private Gold _goldPrefab;
    [SerializeField]
    private Canvas _canvas;

    public bool move;
    public bool onRoad = false;
    public bool runWayPoint = true;

    [HideInInspector]
    public Transform nextWayPoint;

    public List<Transform> WayPoints { get => _wayPoints; }
    public SpriteRenderer SpriteRenderer { get => _spriteRenderer; }
    public Animator Animator { get => _animator; }
    public Enemy Target { get => _target; }
    public BoxCollider2D BodyCollider { get => _bodyCollider; }
    public BoxCollider2D AttackCollider { get => _attackCollider; }
    public bool IsDead {
        get {
            if (health <= 0) {
                return true;
            }

            return false;
        }
    }

    private void Awake() {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        nextWayPoint = _wayPoints[0];
        AddEnbleAttackColliderEventForAttackAnimation();
        AddDisableAttackColliderEventForAttackAnimation();
        AddDisableMinotaurEventForDeadAnimation();
        DisableAttackCollider();
        ChangeState(new MinotaurIdleState());
        SpawGold();
    }

    public void AddEnbleAttackColliderEventForAttackAnimation() {
        float _playingAnimationTime = _frameRateEnableAttackCollider / _attackClip.frameRate;
        _attackEvent.time = _playingAnimationTime;
        _attackEvent.functionName = nameof(EnableAttackCollier);

        _attackClip.AddEvent(_attackEvent);
    }

    public void AddDisableAttackColliderEventForAttackAnimation() {
        float _playingAnimationTime = _frameRateDisableAttackCollider / _attackClip.frameRate;
        _attackEvent.time = _playingAnimationTime;
        _attackEvent.functionName = nameof(DisableAttackCollider);

        _attackClip.AddEvent(_attackEvent);
    }

    private void EnableAttackCollier() {
        _attackCollider.enabled = true;
    }

    private void DisableAttackCollider() {
        _attackCollider.enabled = false;
    }

    public void AddDisableMinotaurEventForDeadAnimation() {
        float _playingAnimationTime = _deadClip.length;
        _deadkEvent.time = _playingAnimationTime;
        _deadkEvent.functionName = nameof(Dead);

        _deadClip.AddEvent(_deadkEvent);
    }

    public void ChangeState(IState newState) {
        if (_currentState != null) {
            _currentState.Exit();
        }
        _currentState = newState;
        _currentState.Enter(this);
    }

    private void SpawGold() {
        _gold = Instantiate(_goldPrefab, transform.position, Quaternion.identity);
        _gold.Init(_gameManager);
        _gold.transform.SetParent(_canvas.transform);
        _gold.transform.localScale = new Vector3(1f, 1f, 1f);
        _gold.transform.position = transform.position;
        _gold.gameObject.SetActive(false);
    }

    void Update() {
        _currentState.Execute();
    }

    public void SetTarget() {
        if (_enemies.Count > 0) {
            _target = _enemies[0];
        }
        else {
            _target = null;
        }
    }

    public void AddEnemy(Enemy enemy) {
        _enemies.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy) {
        _enemies.Remove(enemy);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.TryGetComponent(out Potion potion)) {
            _dialog.ShowChoiseDialog();
            potion.gameObject.SetActive(false);
            SetParametrs(potion.healthMultiplier, potion.damageMultiplier);
            _healthBar.fillAmount = 1f;
        }
    }

    private void SetParametrs(float healthMultiplier, float damageMultiplier) {
        maxHealth *= healthMultiplier;
        health = maxHealth;
        damage *= damageMultiplier;
    }

    public void SpawnGold() {
        _gold.gameObject.SetActive(true);
    }

    public override void TakeDamage(float damage) {
        health -= damage;
        ShiftHealthBar();

        if (IsDead) {
            _target.isUnderAttack = false;
            _target.CheckFlipSprite(_target.NextWayPoint);
            _animator.SetTrigger("dead");
        }
    }

    private void Dead() {
        gameObject.SetActive(false);
    }

    private void ShiftHealthBar() {
        _healthBar.fillAmount = health / maxHealth;
    }

    public void SetLayer() {
        float _different = transform.position.y - _startPosition.y;

        if (_different >= 0.1) {
            _spriteRenderer.sortingOrder -= 1;
            _startPosition = transform.position;
        }

        else if (_different < -0.1) {
            _spriteRenderer.sortingOrder += 1;
            _startPosition = transform.position;
        }
    }

    public void InitStartPosition() {
        _startPosition = transform.position;
    }
}
