using UnityEngine;

public class Range : MonoBehaviour
{
    [SerializeField]
    private Tower _tower;
    [SerializeField]
    private int _ignoreRaycastLayerIndex;

    private void OnEnable() {
        gameObject.layer = _ignoreRaycastLayerIndex;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Enemy _enemy = collision.gameObject.GetComponent<Enemy>();

        if (_enemy != null) {
            _tower.EnemyList.Add(_enemy);
            _enemy.InitializationTower(_tower);

            if (_tower.IsTargetNull()) {
                _tower.SetTarget();
            }

            //print("tower = " + _tower.gameObject.name);
            //print("target = " + _tower.target.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        Enemy _enemy = collision.gameObject.GetComponent<Enemy>();

        if (_enemy != null) {
            _tower.RemoveTarget(_enemy);
            _tower.SetTarget();
            _enemy.GetLastPosition();
            //print("enemy go went");
        }
    }
}
