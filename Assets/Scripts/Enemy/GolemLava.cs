using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemLava : Enemy
{
    [SerializeField]
    private EnemyRange _range;

    private new void Start()
    {
        base.Start();
    }

    private new void Update()
    {
        base.Update();
    }

    public override void TakeDamage(float damage) {
        if (!IsDead) {
            _health -= damage;
            SoundManager.Instance.PlaySoundEffect(SoundName.HitEnemy);
            ShiftHealthBar();

            if (IsDead) {
                CallImmunityBurningInEnemies();
                DeathFromBullet();
            }
        }
    }

    private void CallImmunityBurningInEnemies() {
        foreach (Enemy enemy in _range.Enemies) {
            enemy.Debuff.StartImmunityBurninfForSomeTime();
        }
    }
}
