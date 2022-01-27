public class IronTower : Tower
{
    public bool isPurchasedAbilityIncreaseSpeedShootIronTower;

    private new void Start()
    {
        base.Start();

        CheckPurchaseAbilityAndIncreaseTimeShoot();
    }

    private void CheckPurchaseAbilityAndIncreaseTimeShoot() {
        if (isPurchasedAbilityIncreaseSpeedShootIronTower) {
            _timeShoot /= 2;
            print("time shoot = " + _timeShoot);
        }
    }

    private new void Update()
    {
        base.Update();
    }
}
