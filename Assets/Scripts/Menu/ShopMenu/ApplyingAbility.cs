using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyingAbility : MonoBehaviour
{
    [SerializeField]
    private IronTower _ironTower;
    [SerializeField]
    private Bullet _ironBullet;

    public void ApplyAbility(TypeAbility _typeAbility) {
        switch (_typeAbility) {
            case TypeAbility.IncreaseSpeedShootIronTower:
                _ironTower.isPurchasedAbilityIncreaseSpeedShootIronTower = true;
                break;
        }
    }
}
