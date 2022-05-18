using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CartSpawnInfo : MonoBehaviour
{
    [SerializeField]
    private Image _icon;
    [SerializeField]
    private TextMeshProUGUI _name;
    [SerializeField]
    private TextMeshProUGUI _amount;
    [SerializeField]
    private RectTransform _rectTransform;

    public RectTransform RectTransform { get => _rectTransform; }

    public void Init(CartSpawnInfoData info, int amount) {
        _icon.sprite = info.icon;
        _name.text = info.name;
        _amount.text = "X" + amount;
    }
}
