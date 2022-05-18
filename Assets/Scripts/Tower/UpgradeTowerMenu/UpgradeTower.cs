using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeTower : MonoBehaviour
{
    [SerializeField]
    private int _price;
    [SerializeField]
    private int _priceIncrease;
    [SerializeField]
    private TextMeshProUGUI _priceText;
    [SerializeField]
    private Button _button;
    [SerializeField]
    private GameObject _priceGameObject;

    [HideInInspector]
    public int improvement = 0;
    public int Price { get => _price; }
    public Button Button { get => _button; }

    private void Start() {
        SetPriceText();
    }

    public void SetPriceText() {
        _priceText.text = "" + _price;
    }

    public void IncreasePrice() {
        _price += _priceIncrease;
        SetPriceText();
    }

    public void DisablePrice() {
        _priceGameObject.SetActive(false);
    }
}
