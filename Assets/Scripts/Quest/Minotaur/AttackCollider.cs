using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    [SerializeField]
    private NPCMinotaur _npcMinotaur;

    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision.TryGetComponent(out Enemy enemy)) {
            if (enemy == _npcMinotaur.Target) {
                _npcMinotaur.Target.TakeDamage(_npcMinotaur.damage);
            }
        }

        //print("enter collision");
    }
}
