using System.Collections;
using UnityEngine;

public class EnemyDebuff : MonoBehaviour
{
    private float _time;
    private bool _isSlow = false;
    private bool _isBurning = false;

    [SerializeField]
    private Enemy _enemy;
    [SerializeField]
    protected float _slowSpeed;
    [SerializeField]
    private float _timeBurning;

    private void Start()
    {
        _time = _timeBurning;
    }

    private void Update()
    {
        if (_isBurning) {
            _time -= Time.deltaTime;

            if (_time <= 0) {
                _isBurning = false;
                _time = _timeBurning;
            }
        }
    }

    public void StartSlowMove() {
        if (!_isSlow) {
            _isSlow = true;
            StartCoroutine(SlowMove());
        }
    }

    private IEnumerator SlowMove() {
        while (_isSlow) {
            _enemy.SetSpeed(_slowSpeed);
            _enemy.Animator.SetFloat("speedStateWalking", _slowSpeed);
            yield return new WaitForSeconds(2f);
            _isSlow = false;
        }

        _enemy.InitSpeed();
        _enemy.Animator.SetFloat("speedStateWalking", 1f);
    }

    public void StartBurning() {
        if (!_isBurning) {
            StartCoroutine(Burning());
        }
    }

    private IEnumerator Burning() {
        _isBurning = true;
        _enemy.PlayBurningEffect();

        while (_isBurning) {
            yield return new WaitForSeconds(1f);
            _enemy.TakeDamage(0.5f);
        }

        _enemy.StopBurningEffect();
    }
}
