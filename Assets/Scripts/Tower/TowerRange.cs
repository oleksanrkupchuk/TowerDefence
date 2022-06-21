using UnityEngine;

public class TowerRange : MonoBehaviour {
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

            if (_tower.IsTargetNull()) {
                _tower.SetNewTarget();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.TryGetComponent(out Enemy enemy)) {
            _tower.RemoveTarget(enemy);

            if (_tower.Target == enemy) {
                _tower.SetTargetPosition(enemy.transform);
                //print(_tower.gameObject.name + " enemy go went " + enemy.gameObject.name);
                _tower.SetNewTarget();
            }

            _tower.EnemyOutRange(enemy);
        }
    }
}
