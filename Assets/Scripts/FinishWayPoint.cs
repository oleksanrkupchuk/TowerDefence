using UnityEngine;

public class FinishWayPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        Enemy _enemy = collision.GetComponent<Enemy>();
        if(_enemy != null) {
            _enemy.DeathFromLastWay();
        }
    }
}
