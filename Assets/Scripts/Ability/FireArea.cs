using UnityEngine;

public class FireArea : BulletAbility
{
    //[SerializeField]
    //private Animator _animator;

    [SerializeField]
    private Collider2D _collider;
    [SerializeField]
    private AnimationClip _reductionAnimation;
    [SerializeField]
    private ParticleSystem _fire;

    public void SetDefaultParam() {
        gameObject.transform.localScale = new Vector3(1.5f, 1.5f);
        _fire.transform.localScale = new Vector3(1.5f, 1.5f);
        _time = 5f;
        gameObject.SetActive(true);
    }

    private void OnEnable() {
        Invoke(nameof(DisableColliderAndStartAnimation), _time);
    }

    private void DisableColliderAndStartAnimation() {
        _collider.enabled = false;
        //_animator.SetTrigger("reduction");
        Invoke(nameof(Disable), 1f);
    }

    private void Disable() {
        gameObject.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.TryGetComponent(out Enemy enemy)) {
            enemy.Debuff.StartBurning();
        }
    }
}
