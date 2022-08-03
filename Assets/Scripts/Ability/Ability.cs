using UnityEngine;
using UnityEngine.UI;

public enum AbilityType {
    SpeedShoot,
    Spike,
    Burning,
    FireArea,
    Explosion,
    ReducedPriceTower,
    IncreasedPriceSellTower,
    AccessSpawnInfo
}

public class Ability : MonoBehaviour {
    private AbilityData _data;
    private ShopMenu _shopMenu;
    [Header("Components")]
    [SerializeField]
    private Image _icon;
    [SerializeField]
    private Text _price;
    [SerializeField]
    private Button _button;

    [Header("Parameters")]
    [SerializeField]
    private int _percentageOfTowerPriceReduction;
    [SerializeField]
    private int _percentSellTower;

    public AbilityData Data { get => _data; }
    public int PercentageOfTowerPriceReduction { get => _percentageOfTowerPriceReduction; }
    public int PercentSellTower { get => _percentSellTower; }

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

    public void SetIcon(Sprite icon) {
        _icon.sprite = icon;
    }
}
