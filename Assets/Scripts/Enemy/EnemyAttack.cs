using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField]
    private Enemy _enemy;

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.TryGetComponent(out NPCMinotaur npcMinotaur)) {
            print("take damage = " + _enemy.Damage);
            npcMinotaur.TakeDamage(_enemy.Damage);
        }
    }
}
