using System;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
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
    [SerializeField]
    private FindingWay _findingWay;

    [Header("Components")]
    [SerializeField] 
    private BoxCollider2D _boxCollider;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [Header("GameObjects")]
    [SerializeField]
    private GameObject _healthBarBackground;


    [Header("UI")]
    [SerializeField]
    private RectTransform _healthBarRectTransform;
    [SerializeField]
    private Image _healthBar;

    private ListEnemys _enemyList;

    [Header("Animator parametrs")]
    private string _isDying = "isDying";

    private void Awake() {
        if(_boxCollider == null) {
            Debug.LogError("component is null");
        }

        _healthBarBackground.SetActive(true);

        _enemyList = FindObjectOfType<ListEnemys>();
        _enemyList.AddEnemy(gameObject.GetComponent<Enemy>());
    }

    private void Update() {
        if (!isDead) {
            _findingWay.FindingPath();

            Move();
        }
    }

    private void Move() {
        transform.position = Vector3.MoveTowards(transform.position, _findingWay.NextPosition, _speed * Time.deltaTime);
    }

    public void TakeDamage(int damage) {
        print("--------------");
        print("HIT = " + damage);
        if(_health > 0) {
            _health -= damage;
            ShiftHealthBar();

            if (isDead) {
                Dying();
            }
        }
    }

    private void ShiftHealthBar() {
        _healthBar.fillAmount = (float)_health/_healthMax;
    }

    private void Dying() {
        diePosition = transform.position;
        GameManager.Instance.AddCoin(_coinForDeath);
        _enemyList.RemoveEnemy(gameObject.GetComponent<Enemy>());
        _animator.SetTrigger(_isDying);
        _healthBarBackground.SetActive(false);
        //gameObject.SetActive(false);
        _boxCollider.enabled = false;
        _spriteRenderer.sortingOrder = _sortingLayer;
        if(_enemyList.ListEnemy.Count <= 0) {
            EnemySpawner.Instance.ResetEnemyQuantityInSpawn();
            EnemySpawner.Instance.StartSpawn();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Tags.finish))
        {
            _findingWay.stopMovement = true;
            Dying();
        }
    }
}
