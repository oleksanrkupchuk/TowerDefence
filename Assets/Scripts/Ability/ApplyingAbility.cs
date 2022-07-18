using UnityEngine;

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
    [SerializeField]
    private SpawnInfo _spawnInfo;

    private void Awake() {
        if (_applyingAbility == null) {
            _applyingAbility = this;
        }
        else if (_applyingAbility != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    //private void Start() {
    //    AbilityPurchased _abilityPurchased = AbilitySaveSystem.LoadAbility();
    //    for (int i = 0; i < _abilityPurchased.abilities.Count; i++) {
    //        if (_abilityPurchased.abilities[i].type != AbilityType.ReducePriceTower) {
    //            CheckPurchasedAbility(_abilityPurchased.abilities[i]);
    //        }
    //    }
    //}

    //private void CheckPurchasedAbility(AbilityItem item) {
    //    if (item.isPurchased == true) {
    //        ApplyAbility(item.type);
    //    }
    //}

    public void ApplyAbility(AbilityType type) {
        switch (type) {
            case AbilityType.SpeedShoot:
                _ironTower.isBuyAbility = true;
                break;

            case AbilityType.Spike:
                _ironBullet.thonr = true;
                break;

            case AbilityType.Burning:
                _fireBullet.burning = true;
                break;

            case AbilityType.FireArea:
                _fireBullet.fireArea = true;
                break;

            case AbilityType.Explosion:
                _rockBullet.isExplosion = true;
                break;

            case AbilityType.IncreasedPriceSellTower:
                _towerUpgradeMenu.percentSellTower = _ability.PercentSellTower;
                break;

            case AbilityType.ReducedPriceTower:
                _ironTower.ReducePrice(_ability.PercentageOfTowerPriceReduction);
                _fireTower.ReducePrice(_ability.PercentageOfTowerPriceReduction);
                _rockTower.ReducePrice(_ability.PercentageOfTowerPriceReduction);
                break;

            case AbilityType.AccessSpawnInfo:
                _spawnInfo.isAccessSpawnInfo = true;
                break;
        }
    }

}
