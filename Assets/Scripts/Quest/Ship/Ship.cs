using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Ship : MonoBehaviour {
    private BoxCollider2D _boxCollider;
    private bool _isFlipLeft = false;
    private List<ExplosionCannon> _explosions = new List<ExplosionCannon>();

    [SerializeField]
    private float _speed;
    [SerializeField]
    private Sight _sight;
    [SerializeField]
    private List<Transform> _wayPoints;
    [SerializeField]
    private Transform _goAwayPoint;
    [SerializeField]
    private ParticleSystem[] _shoots;
    [SerializeField]
    private ExplosionCannon _explosionCannonPrefab;
    [SerializeField]
    private int _priceForService;
    [SerializeField]
    private ShipDialog _shipDialog;

    [HideInInspector]
    public Transform nextWayPoint;
    [HideInInspector]
    public int indexPosition;
    [HideInInspector]
    public List<Vector2> positionForExplosions = new List<Vector2>();
    [HideInInspector]
    public bool canBuyService = true;
    public List<Transform> WayPoints { get => _wayPoints; }
    public int PriceForService { get => _priceForService; }

    private void Awake() {
        _boxCollider = GetComponent<BoxCollider2D>();
        _boxCollider.isTrigger = true;

        SpawnExplosionCannon();
    }

    private void SpawnExplosionCannon() {
        for (int i = 0; i < 7; i++) {
            ExplosionCannon _explosion = Instantiate(_explosionCannonPrefab, transform.position, _explosionCannonPrefab.transform.rotation);
            _explosion.gameObject.SetActive(false);
            _explosions.Add(_explosion);
        }
    }

    private void Start() {
        nextWayPoint = _wayPoints[0];
    }

    private void Update() {
        GetNextPosition();
        Move(nextWayPoint);
    }

    public void Move(Transform toObject) {
        transform.position = Vector2.MoveTowards(transform.position, toObject.position, _speed * Time.deltaTime);
    }

    public void GetNextPosition() {
        if (Vector2.Distance(transform.position, nextWayPoint.position) <= 0.02f) {

            indexPosition++;
            if (indexPosition < WayPoints.Count) {
                nextWayPoint = WayPoints[indexPosition];
            }
            else {
                indexPosition = 0;
                nextWayPoint = WayPoints[0];
            }

            CheckFlipSprite(nextWayPoint);
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

    private void FlipSprite() {
        _isFlipLeft = !_isFlipLeft;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y);
    }

    private void OnMouseDown() {
        if (canBuyService) {
            _shipDialog.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.TryGetComponent(out Potion potion)) {
            EnableSight();
        }
    }

    public void ShootCannon() {
        foreach (ParticleSystem shoot in _shoots) {
            shoot.gameObject.SetActive(true);
            shoot.Play();
        }
    }

    public void EnableSight() {
        _sight.gameObject.SetActive(true);
    }

    public IEnumerator EnableExplosionCannon() {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < _explosions.Count; i++) {
            _explosions[i].transform.position = positionForExplosions[i];
            _explosions[i].gameObject.SetActive(true);
            //print($"pos {i} = " + positionForExplosions[i]);
            yield return new WaitForSeconds(.3f);
        }

        _sight.gameObject.SetActive(false);
    }

    public void SetGoAwayPointAndMove() {
        nextWayPoint = _goAwayPoint;
        CheckFlipSprite(nextWayPoint);
        Move(nextWayPoint);
    }
}
