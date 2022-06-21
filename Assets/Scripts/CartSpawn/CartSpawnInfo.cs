using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CartSpawnInfo : MonoBehaviour
{
    [SerializeField]
    private Image _icon;
    [SerializeField]
    private Text _name;
    [SerializeField]
    private RectTransform _rectTransform;

    public RectTransform RectTransform { get => _rectTransform; }

    public void Init(CartSpawnInfoData info) {
        _icon.sprite = info.icon;
        _name.text = info.name;
    }
}
