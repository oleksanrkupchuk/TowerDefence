using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelCart : MonoBehaviour {
    [SerializeField]
    private Sprite _iconStarFull;
    [SerializeField]
    private Image[] _iconStarsEmpty;
    [SerializeField]
    private Button _button;
    [SerializeField]
    private Text _title;
    [SerializeField]
    private Color _color;
    [SerializeField]
    private Color _colorNormal;

    public bool isUnlock;
    public Button Button { get => _button; }
    public Text Title { get => _title; }

    public void CheckUnlockLevelAndSetIntractable(bool isUnlock) {
        if (!isUnlock) {
            _button.interactable = false;
            SetEclipsForStarsIcon();
        }
        else {
            for (int i = 0; i < _iconStarsEmpty.Length; i++) {
                _iconStarsEmpty[i].color = _colorNormal;
            }
        }
    }

    private void SetEclipsForStarsIcon() {
        for (int i = 0; i < _iconStarsEmpty.Length; i++) {
            _iconStarsEmpty[i].color = _color;
        }
    }

    public void SetIconStarsFull(int count) {
        for (int i = 0; i < count; i++) {
            _iconStarsEmpty[i].sprite = _iconStarFull;
        }
    }
}
