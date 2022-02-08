using UnityEngine;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
    [SerializeField]
    private Tower _tower;
    [SerializeField]
    private Image _icon;
    [SerializeField] 
    private Sprite _towerSprite;
    [SerializeField]
    private Button _button;

    private void Start() {
        _icon.sprite = _towerSprite;
    }

    public Tower TowerScript {
        get {
            return _tower;
        }
    }

    public Sprite Sprite
    {
        get
        {
            return _towerSprite;
        }
    }

    public Button Button { get => _button; }
}
