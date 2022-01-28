using UnityEngine;

public class IronBullet : Bullet
{
    private new void OnEnable() {
        base.OnEnable();
    }

    private new void Update()
    {
        base.Update();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag(Tags.enemy)) {
            if (_target != null) {
                _circleCollider.enabled = false;
                //print("bullet" + gameObject.name + " = " + "damage " + _damage);
                _target.TakeDamage(_damage);
                _lastTargetPosition = _target.transform.position;
                _isChangeTarget = true;
            }
        }
    }
}
