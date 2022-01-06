using UnityEngine;
using TMPro;

public class UpgradeTower : MonoBehaviour
{
    [SerializeField]
    private int _price;
    public int Price { get => _price; }

    [SerializeField]
    private TextMeshProUGUI _priceText;

    [SerializeField]
    private int _priceIncrease;

    private void Start() {
        UpdatePrice();
    }

    public void UpdatePrice() {
        _priceText.text = "" + _price;
    }

    public void IncreasePrice() {
        _price += _priceIncrease;
        UpdatePrice();
    }

    public void NotIntractable() {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
}
