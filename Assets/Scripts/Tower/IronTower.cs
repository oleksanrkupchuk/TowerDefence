public class IronTower : Tower
{
    public bool isBuyAbility;

    private new void Start()
    {
        base.Start();

        CheckPurchaseAbilityAndIncreaseTimeShoot();
    }

    private void CheckPurchaseAbilityAndIncreaseTimeShoot() {
        if (isBuyAbility) {
            _timeShoot /= 2;
            //print("time shoot = " + _timeShoot);
        }
    }

    private new void Update()
    {
        base.Update();
    }

    protected override void PlayShootSound() {
        SoundManager.Instance.PlaySoundEffect(SoundName.IronShot);
    }
}
