using UnityEngine;

public class FireArea : MonoBehaviour
{
    private bool _startReductionAnimation = false;

    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private float _timeLife;

    private AnimationEvent _disableColliderEvent = new AnimationEvent();
    private AnimationEvent _destroyEvent = new AnimationEvent();

    [SerializeField]
    private Collider2D _collider;
    [SerializeField]
    private AnimationClip _reductionAnimation;

    private void Update() {
        if (!_startReductionAnimation) {
            _timeLife -= Time.deltaTime;
            if (_timeLife <= 0) {
                _startReductionAnimation = true;
                _animator.SetTrigger("reduction");
            }
        }
    }

    public void AddEventDisableColliderForReductionAnimation() {
        float _playingAnimationTime = 0f;
        _disableColliderEvent.time = _playingAnimationTime;
        _disableColliderEvent.functionName = nameof(DisableCollider);

        _reductionAnimation.AddEvent(_disableColliderEvent);
    }

    private void DisableCollider() {
        _collider.enabled = false;
    }

    public void AddEventDestroyForReductionAnimation() {
        float _playingAnimationTime = _reductionAnimation.length;
        _destroyEvent.time = _playingAnimationTime;
        _destroyEvent.functionName = nameof(DestroyObjectAfterSomeTime);

        _reductionAnimation.AddEvent(_destroyEvent);
    }

    private void DestroyObjectAfterSomeTime() {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.TryGetComponent(out Enemy enemy)) {
            enemy.Debuff.StartBurning();
        }
    }
}
