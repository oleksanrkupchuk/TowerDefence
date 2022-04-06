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
    private int _percentReductionInPriceTower;
    [SerializeField]
    private int _percentSellTower;
    [SerializeField]
    private int _chanceFireArea;
    [SerializeField]
    private int _chanceExplosion;

    public AbilityData Data { get => _data; }
    public int PercentReductionInPriceTower { get => _percentReductionInPriceTower; }
    public int PercentSellTower { get => _percentSellTower; }
    public int ChanceFireArea { get => _chanceFireArea; }
    public int ChanceExplosion { get => _chanceExplosion; }

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
            _shopMenu.SetCurrentAbilityAndUpdateDetails(this);
            _shopMenu.CheckPurchasedAbilityAndEnableOrDisableBuyButton();
        });
    }
}
