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

    private void OnTriggerEnter2D(Collider2D collision) {
        _towerManager = collision.GetComponent<TowerManager>();
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if(_towerManager != null) {
            if(_towerManager.TowerButtonPressed == null) {
                return;
            }
            _icon.sprite = _towerManager.TowerButtonPressed.Sprite;
            _icon.color = _alpha;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        DisableIlluminationIcon();
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
