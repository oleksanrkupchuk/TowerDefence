using UnityEngine;

public class Explosion : BulletAbility {
    [SerializeField]
    private CircleCollider2D _collider;

    public void SetParametrsToDefault() {
        _collider.enabled = true;
        gameObject.SetActive(true);
    }

    private void OnEnable() {
        Invoke(nameof(Disable), 1f);
    }

    private void Disable() {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.TryGetComponent(out Enemy enemy)) {
            enemy.Debuff.TakeDamage(3);
        }

        _collider.enabled = false;
    }
}
