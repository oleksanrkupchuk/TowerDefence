using UnityEngine;

public class Range : MonoBehaviour {
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
            //print("Set enemy");
            //print("tower = " + _tower.gameObject.name);
            //print("target = " + _tower.GetTarget().name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        Enemy _enemy = collision.gameObject.GetComponent<Enemy>();
        if (_tower.GetTarget() != null) {
            if (_tower.GetTarget() == _enemy) {
                _tower.GetTarget().GetLastPosition();
                _tower.SetPositionTarget(_enemy.transform);
                _tower.RemoveTarget(_enemy);
                _tower.SetTarget();
                //print("enemy go went");
            }
        }
    }
}
