using UnityEngine;

public class PlaceForTower : MonoBehaviour
{
    private TowerManager _towerManager;

    [SerializeField]
    private SpriteRenderer _icon;
    [SerializeField]
    private Color _alpha;

    private void OnTriggerEnter2D(Collider2D collision) {
        _towerManager = collision.GetComponent<TowerManager>();
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if(_towerManager.TowerButtonPressed != null) {
            Sprite _towerSprite = _towerManager.TowerButtonPressed.Sprite;
            _icon.sprite = _towerSprite;
            _icon.color = _alpha;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        SetIconNull();
    }

    public void SetIconNull() {
        _icon.sprite = null;
    }
}