using UnityEngine;

public class PlaceForTower : MonoBehaviour {
    private TowerManager _towerManager;
    private SpriteRenderer _rangeSpriteRenderer;

    [SerializeField]
    private SpriteRenderer _icon;
    [SerializeField]
    private Color _alpha;
    [SerializeField]
    private BoxCollider2D _boxCollider;
    [SerializeField]
    private GameObject _range;

    private void Start() {
        _range.SetActive(false);
        _rangeSpriteRenderer = _range.GetComponent<SpriteRenderer>();
        _rangeSpriteRenderer.sortingOrder = 900;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        _towerManager = collision.GetComponent<TowerManager>();
        if (_towerManager == null) {
            return;
        }
        if (_towerManager.TowerButtonPressed == null) {
            return;
        }
        _icon.sprite = _towerManager.TowerButtonPressed.Sprite;
        _icon.color = _alpha;
        float _radius = _towerManager.TowerButtonPressed.Tower.RangeAttack;
        _range.SetActive(true);
        _range.transform.localScale = new Vector2(_radius * 2, _radius * 2);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        DisableIlluminationIcon();
        _range.SetActive(false);
    }

    public void DisableIlluminationIcon() {
        _icon.sprite = null;
    }

    public void DisableBoxCollider() {
        _boxCollider.enabled = false;
    }

    public void EnableBoxCollider() {
        _boxCollider.enabled = true;
    }
}
