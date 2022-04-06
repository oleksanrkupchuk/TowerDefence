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
            _enemy.SetTower(_tower);

            if (_tower.IsTargetNull()) {
                _tower.SetTarget();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.TryGetComponent(out Enemy enemy)) {
            if (_tower.Target == enemy) {
                //_tower.GetTarget().GetLastPosition();
                _tower.SetTargetPosition(enemy.transform);
                //_tower.Target.GetLastPosition();
                _tower.RemoveBulletsAndSetTargetNull();
                print(_tower.gameObject.name + " enemy go went " + enemy.gameObject.name);
            }
            //enemy.GetLastPosition();
            _tower.RemoveTarget(enemy);
            _tower.SetTarget();
        }
    }
}
