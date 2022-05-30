using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMinotaurRange : MonoBehaviour {
    [SerializeField]
    private NPCMinotaur _npcMinotaur;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.TryGetComponent(out Enemy enemy)) {
            _npcMinotaur.AddEnemy(enemy);
        }

        if (_npcMinotaur.Target == null) {
            _npcMinotaur.SetTarget();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.TryGetComponent(out Enemy enemy)) {
            _npcMinotaur.RemoveEnemy(enemy);

            if(enemy == _npcMinotaur.Target) {
                _npcMinotaur.SetTarget();
            }
        }
    }
}
