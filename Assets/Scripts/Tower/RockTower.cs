public class RockTower : Tower
{
    private new void Start()
    {
        base.Start();
    }

    private new void Update()
    {
        base.Update();
    }

    protected override void PlayShootSound() {
        SoundManager.Instance.PlaySoundEffect(SoundName.RockShot);
    }
}
