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
    protected Vector2 _previousPosition;
    protected Vector2 _startPosition;
    protected bool _isFlipLeft = false;
    protected ParticleSystemRenderer _burningRenderer;
    protected ParticleSystemRenderer _healingRenderer;

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
    protected string _isDead = "isDying";
    protected List<Transform> _currentWay = new List<Transform>();
    protected AnimationEvent _enemyEventDead = new AnimationEvent();
    protected AnimationEvent _attackEvent = new AnimationEvent();
    protected float _defaultSpeed;
    protected float _healthMax;

    [Header("Parametrs")]
    [SerializeField]
    protected float _health;
    [SerializeField]
    protected float _speed;
    [SerializeField]
    protected float _damage;
    [SerializeField]
    protected int _amountCoinForDeath;

    [Header("Components")]
    [SerializeField]
    protected Canvas _canvas;
    [SerializeField]
    protected EnemyCartData _enemyCartData;
    [SerializeField]
    private BoxCollider2D _attackCollider;

    [Header("UI")]
    [SerializeField]
    protected Image _healthBar;

    [Header("Animator parametrs")]
    [SerializeField]
    protected AnimationClip _deadClip;
    [SerializeField]
    protected AnimationClip _attackClip;
    [SerializeField]
    protected AnimationClip _walkingClip;
    [SerializeField]
    private AnimationState _animationState;
    [SerializeField]
    private int _frameRateEnableAttackCollider;
    [SerializeField]
    private int _frameRateDisableAttackCollider;

    [Header("Effects")]
    [SerializeField]
    protected ParticleSystem _healingEffect;
    [SerializeField]
    private ParticleSystem _burningEffect;

    [HideInInspector]
    public Vector3 lastPosition;

    public bool isUnderAttack;

    public float Health { get => _health; }
    public float Speed { get => _defaultSpeed; }
    public float Damage { get => _damage; }
    public bool IsDead { get => _health <= 0; }
    public Animator Animator { get => _animator; }
    public Transform NextWayPoint { get => _nextWayPoint; }
    public EnemyDebuff Debuff { get => _debuff; }
    public EnemyCartData EnemyCartData { get => _enemyCartData; }
    public CircleCollider2D CircleCollider2D { get => _collider; }
    public BoxCollider2D AttackCollider { get => _attackCollider; }

    public static event Action<Enemy> Dead;

    public void Init(GameManager gameManager, EnemySpawner enemySpawner, Camera camera) {
        _gameManager = gameManager;
        _enemySpawner = enemySpawner;
        _camera = camera;

        GetComponents();

        _rigidbody.gravityScale = 0f;
        _collider.isTrigger = true;
        _collider.radius = 0.25f;
        _collider.offset = new Vector2(0f, 0.2f);
        _attackCollider.enabled = false;
    }

    protected void GetComponents() {
        _collider = GetComponent<CircleCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _debuff = GetComponent<EnemyDebuff>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _burningRenderer = _burningEffect.GetComponent<ParticleSystemRenderer>();
        _healingRenderer = _healingEffect.GetComponent<ParticleSystemRenderer>();
    }

    protected void Start() {
        SetHealthToDefault();
        SetSpeedToDefault();
        StopBurningEffect();
        _startPosition = transform.position;
    }

    public void DisableBetweetCollider(Collider2D colliderOne, Collider2D colliderTwo) {
        Physics2D.IgnoreCollision(colliderOne, colliderTwo, true);
    }

    public void SetHealthToDefault() {
        _healthMax = _health;
    }

    public void SetSpeedToDefault() {
        _defaultSpeed = _speed;
    }

    public void AddDestroyEventForDeadAnimation() {
        float _playingAnimationTime = _deadClip.length;
        _enemyEventDead.time = _playingAnimationTime;
        _enemyEventDead.functionName = nameof(DisableEnemyAfterTime);

        _deadClip.AddEvent(_enemyEventDead);
    }

    public void AddEnableEventForAttackAnimation() {
        float _playingAnimationTime = _frameRateEnableAttackCollider / _attackClip.frameRate;
        _attackEvent.time = _playingAnimationTime;
        _attackEvent.functionName = nameof(EnableAttackCollier);

        _attackClip.AddEvent(_attackEvent);
    }

    public void AddDisableEventForAttackAnimation() {
        float _playingAnimationTime = _frameRateDisableAttackCollider / _attackClip.frameRate;
        _attackEvent.time = _playingAnimationTime;
        _attackEvent.functionName = nameof(DisableAttackCollider);

        _attackClip.AddEvent(_attackEvent);
    }

    private void EnableAttackCollier() {
        //print("attack enable");
        _attackCollider.enabled = true;
    }

    private void DisableAttackCollider() {
        _attackCollider.enabled = false;
    }

    protected void Update() {
        if (!IsDead) {
            if (_currentWay != null && !isUnderAttack) {
                GetNextPosition();
                Move();
                SetLayer();
                SetLayerEffects();
            }

            CheckUnderAttack();
        }
    }

    protected void GetNextPosition() {
        if (Vector2.Distance(transform.position, _nextWayPoint.position) <= 0.02f) {

            _indexPosition++;
            if (_indexPosition < _currentWay.Count) {
                _nextWayPoint = _currentWay[_indexPosition];
            }

            CheckFlipSprite(_nextWayPoint);
        }
    }

    public void CheckFlipSprite(Transform target) {
        if (transform.position.x - target.position.x < 0 && _isFlipLeft) {
            FlipSprite();
        }

        if (transform.position.x - target.position.x > 0 && !_isFlipLeft) {
            FlipSprite();
        }
    }

    protected void FlipSprite() {
        _isFlipLeft = !_isFlipLeft;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y);
    }

    protected void Move() {
        transform.position = Vector2.MoveTowards(transform.position, _nextWayPoint.position, _speed * Time.deltaTime);
    }

    protected void SetLayer() {
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

    protected void CheckUnderAttack() {
        if (isUnderAttack) {
            _animator.SetBool("isUnderAttack", true);
        }
        else {
            _animator.SetBool("isUnderAttack", false);
        }
    }

    public virtual void TakeDamage(float damage) {
        if (!IsDead) {
            _health -= damage;
            SoundManager.Instance.PlaySoundEffect(SoundName.HitEnemy);
            ShiftHealthBar();

            if (IsDead) {
                DeadFromBullet();
            }
        }
    }

    protected void ShiftHealthBar() {
        _healthBar.fillAmount = _health / _healthMax;
    }

    protected void DeadFromBullet() {
        Dead?.Invoke(this);
        _collider.enabled = false;
        _enemySpawner.RemoveEnemyInCurrentWave();
        DisableHealthBar();
        PlayDeadAnimation();
        _gameManager.AddCoin(_amountCoinForDeath);
        _gameManager.CheckLastEnemyAndEnableWinMenuOrSpawnNewEnemyWave();
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
        Invoke(nameof(DisableEnemy), 0.2f);
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

    public float CalculationAdditionalHealth(float percentageOfRecovery) {
        float _recoveryHealth = ((percentageOfRecovery * _healthMax) / 100);
        return _recoveryHealth;
    }

    public virtual void AddHealth(float additionalHealth) {
        _health += additionalHealth;
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

    public void SetLayerEffects() {
        _burningRenderer.sortingOrder = _spriteRenderer.sortingOrder + 1;
        _healingRenderer.sortingOrder = _spriteRenderer.sortingOrder + 2;
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
