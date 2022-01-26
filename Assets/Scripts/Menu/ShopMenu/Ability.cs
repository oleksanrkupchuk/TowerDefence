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
    private AbilityData _abilityData;
    private ShopMenu _shopMenu;

    [Header("Components")]
    [SerializeField]
    private Image _icon;
    [SerializeField]
    private TextMeshProUGUI _price;
    [SerializeField]
    private Button _button;

    public AbilityData AbilityData { get => _abilityData; }

    public void InitializationAbility(AbilityData abilityData, ShopMenu shopMenu) {
        _abilityData = abilityData;
        _shopMenu = shopMenu;

        CheckForPurchasedAbilityAndSetIcon(_abilityData);

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
        });
    }

    private void SetAbilityDetails() {
        _shopMenu.SetDetailsAbilityIcon(_abilityData.icon);
        _shopMenu.SetDetailsAbilityDescription(_abilityData.description);
    }
}
