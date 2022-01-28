using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum TypeAbility {
    IncreaseSpeedShootIronTower,
    ThornsIronTower,
    BurningFireTower,
    FireAreaFireTower,
    ExplosionRockTower,
    ReducePriceTower,
    IncreasePriceWhenSellTower
}

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

    public AbilityData Data { get => _data; }

    public void InitializationAbility(AbilityData abilityData, ShopMenu shopMenu) {
        _data = abilityData;
        _shopMenu = shopMenu;

        CheckForPurchasedAbilityAndSetIcon(_data);

        _price.text = "" + abilityData.price;

        SubscriptionButton();
    }

    public void CheckForPurchasedAbilityAndSetIcon(AbilityData abilityData) {
        if (abilityData.isPurchased) {
            _icon.sprite = abilityData.iconAfterPurchased;
        }
        else {
            _icon.sprite = abilityData.icon;
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
