using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class ExplosionCannon : MonoBehaviour
{
    private CircleCollider2D _circleCollider;

    [SerializeField]
    private float _damage;
    [SerializeField]
    private List<ParticleSystem> _particle = new List<ParticleSystem>();

    private void OnEnable() {
        _circleCollider = GetComponent<CircleCollider2D>();
        _circleCollider.isTrigger = true;

        foreach (var particle in _particle) {
            particle.Play();
        }

        Invoke(nameof(DisableCircleCollider), 0.1f);
    }

    private void DisableCircleCollider() {
        _circleCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.TryGetComponent(out Enemy enemy)) {
            enemy.TakeDamage(_damage);
        }
    }
}
