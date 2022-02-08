using UnityEngine;

public enum AbilityType {
    IronTower,
    Thorn,
    Burning,
    FireArea,
    Explosion,
    ReducePriceTower,
    IncreasePriceSell
}

public class ApplyingAbility : MonoBehaviour {
    private static ApplyingAbility _applyingAbility;
    [SerializeField]
    private IronTower _ironTower;
    [SerializeField]
    private FireTower _fireTower;
    [SerializeField]
    private RockTower _rockTower;
    [SerializeField]
    private IronBullet _ironBullet;
    [SerializeField]
    private FireBullet _fireBullet;
    [SerializeField]
    private RockBullet _rockBullet;
    [SerializeField]
    private TowerUpgradeMenu _towerUpgradeMenu;
    [SerializeField]
    private Ability _ability;

    private void Awake() {
        //_ironTower.ReducePrice(_ability.PercentageReductionInPriceTower);
        //_fireTower.ReducePrice(_ability.PercentageReductionInPriceTower);
        //_rockTower.ReducePrice(_ability.PercentageReductionInPriceTower);
        if (_applyingAbility == null) {
            _applyingAbility = this;
        }
        else if(_applyingAbility != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        if (AbilitySaveSystem.IsExistsSaveAbilityFile()) {
            AbilityPurchased _abilityPurchased = AbilitySaveSystem.LoadAbility();
            for (int i = 0; i < _abilityPurchased.abilities.Count; i++) {
                CheckPurchasedAbility(_abilityPurchased.abilities[i]);
            }
        }
    }

    private void CheckPurchasedAbility(AbilityItem item) {
        if (item.isPurchased == true) {
            ApplyAbility(item.type);
        }
    }

    public void ApplyAbility(AbilityType type) {
        switch (type) {
            case AbilityType.IronTower:
                _ironTower.isBuyAbility = true;
                break;

            case AbilityType.Thorn:
                _ironBullet.thonr = true;
                break;

            case AbilityType.Burning:
                _fireBullet.burning = true;
                break;

            case AbilityType.FireArea:
                _fireBullet.fireArea = true;
                break;

            case AbilityType.Explosion:
                _rockBullet.AddEventExplosionForDestroyAnimation();
                break;

            case AbilityType.IncreasePriceSell:
                _towerUpgradeMenu.percentSellTower = _ability.PercentageSellTower;
                break;

            case AbilityType.ReducePriceTower:
                _ironTower.ReducePrice(_ability.PercentageReductionInPriceTower);
                _fireTower.ReducePrice(_ability.PercentageReductionInPriceTower);
                _rockTower.ReducePrice(_ability.PercentageReductionInPriceTower);
                break;
        }
    }

}
