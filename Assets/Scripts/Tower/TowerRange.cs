using UnityEngine;

public class TowerRange: MonoBehaviour
{
    [SerializeField]
    private Tower _tower;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<Enemy>()) {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            _tower.EnemyList.Add(enemy);
            enemy.Tower = _tower;
            if (_tower.EnemyList.Count == 1) {
                _tower.target = _tower.EnemyList[0];
            }

            print("tower = " + _tower.gameObject.name);
            print("target = " + _tower.target.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag(Tags.bullet)) {
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.GetComponent<Enemy>()) {
            _tower.RemoveTarget(collision.gameObject.GetComponent<Enemy>());
            _tower.SetTarget();
        }
    }
}
