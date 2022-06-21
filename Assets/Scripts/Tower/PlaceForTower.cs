using UnityEngine;

public class PlaceForTower : MonoBehaviour
{
    private TowerManager _towerManager;

    [SerializeField]
    private SpriteRenderer _icon;
    [SerializeField]
    private Color _alpha;
    [SerializeField]
    private BoxCollider2D _boxCollider;
    [SerializeField]
    private LineRenderer _lineRenderer;

    private void Start() {
        _lineRenderer.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        _towerManager = collision.GetComponent<TowerManager>();
        if (_towerManager != null) {
            if (_towerManager.TowerButtonPressed == null) {
                return;
            }

            _icon.sprite = _towerManager.TowerButtonPressed.Sprite;
            _icon.color = _alpha;
            float _radius = _towerManager.TowerButtonPressed.Tower.RangeAttack;
            _towerManager.TowerButtonPressed.Tower.SetRadiusInLineRanderer(transform, _lineRenderer, _radius);
            _lineRenderer.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        DisableIlluminationIcon();
        _lineRenderer.enabled = false;
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
