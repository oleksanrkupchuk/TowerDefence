using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Ability : MonoBehaviour {
    private AbilityData _data;
    private ShopMenu _shopMenu;

    [Header("Components")]
    [SerializeField]
    private Image _icon;
    [SerializeField]
    private TextMeshProUGUI _price;
    [SerializeField]
    private Button _button;

    [Header("Parameters")]
    [SerializeField]
    private int _percentageReductionInPriceTower;
    [SerializeField]
    private int _percentageSellTower;
    [SerializeField]
    private int _chanceFireArea;

    public AbilityData Data { get => _data; }
    public int PercentageReductionInPriceTower { get => _percentageReductionInPriceTower; }
    public int PercentageSellTower { get => _percentageSellTower; }
    public int ChanceFireArea { get => _chanceFireArea; }

    public void Init(AbilityData abilityData, ShopMenu shopMenu) {
        _data = abilityData;
        _shopMenu = shopMenu;

        _price.text = "" + abilityData.price;

        SubscriptionButton();
    }

    public void CheckForPurchasedAbilityAndSetIcon() {
        if (_data.isPurchased) {
            _icon.sprite = _data.iconAfterPurchased;
        }
        else {
            _icon.sprite = _data.icon;
        }
    }

    private void SubscriptionButton() {
        _button.onClick.AddListener(() => {
            SetAbilityDetails();
            _shopMenu.SetCurrentAbility(this);
            _shopMenu.CheckPurchasedAbilityAndEnableOrDisableBuyButton();
        });
    }

    private void SetAbilityDetails() {
        _shopMenu.SetDetailsAbilityIcon(_data.icon);
        _shopMenu.SetDetailsAbilityDescription(_data.description);
    }
}
