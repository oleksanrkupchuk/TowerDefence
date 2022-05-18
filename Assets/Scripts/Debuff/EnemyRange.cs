using System.Collections.Generic;
using UnityEngine;

public class EnemyRange : MonoBehaviour
{
    [SerializeField]
    private List<Enemy> _enemies = new List<Enemy>();
    [SerializeField]
    private CircleCollider2D _circleCollider;
    [SerializeField]
    private float _radius;

    public List<Enemy> Enemies { get => _enemies; }

    private void Start() {
        _circleCollider.isTrigger = true;
        _circleCollider.radius = _radius;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.TryGetComponent(out Enemy enemy)) {
            _enemies.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.TryGetComponent(out Enemy enemy)) {
            _enemies.Remove(enemy);
        }
    }
}
