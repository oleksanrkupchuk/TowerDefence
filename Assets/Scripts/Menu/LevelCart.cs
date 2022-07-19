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
    private Color _fadeColor;
    [SerializeField]
    private Color _normalColor;

    public bool isUnlock;
    public Button Button { get => _button; }
    public Text Title { get => _title; }

    public void CheckUnlockLevelAndSetIntractable(bool isUnlock, int amountStars) {
        if (!isUnlock) {
            _button.interactable = false;
            SetEclipsForStarsIcon(3, _fadeColor);
        }
        else {
            SetEclipsForStarsIcon(amountStars, _normalColor);
        }
    }

    private void SetEclipsForStarsIcon(int amountStars, Color color) {
        for (int i = 0; i < amountStars; i++) {
            _iconStarsEmpty[i].color = color;
        }
    }
}
