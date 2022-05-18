using UnityEngine;

public class Thorn : BulletAbility
{
    public void SetParametrsToDefault() {
        _time = 4f;
        gameObject.SetActive(true);
    }

    private void OnEnable() {
        Invoke(nameof(Disable), _time);
    }

    private void Disable() {
        gameObject.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.TryGetComponent(out Enemy enemy)) {
            enemy.Debuff.StartSlowMove();
        }
    }
}
