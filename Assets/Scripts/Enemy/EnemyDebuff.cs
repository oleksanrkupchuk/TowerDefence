using System.Collections;
using UnityEngine;

public class EnemyDebuff : MonoBehaviour {
    private float _time;
    private bool _isSlow = false;
    private bool _isBurning = false;

    [SerializeField]
    private Enemy _enemy;
    [SerializeField]
    private bool _immunitySlow;
    [SerializeField]
    private bool _immunityBurning;

    [Header("Debuff Slow")]
    [SerializeField]
    private float _slowSpeed;
    [SerializeField]
    private float _durationSlowSpeed;

    [Header("Debuff Burning")]
    [SerializeField]
    private float _durationBurning;

    private void Start() {
        _time = _durationBurning;
    }

    private void Update() {
        if (_isBurning) {
            _time -= Time.deltaTime;

            if (_time <= 0) {
                _isBurning = false;
                _time = _durationBurning;
            }
        }
    }

    public void StartSlowMove() {
        if (_immunitySlow) {
            return;
        }

        if (!_isSlow) {
            _isSlow = true;
            StartCoroutine(SlowMove());
        }
    }

    private IEnumerator SlowMove() {
        while (_isSlow) {
            _enemy.SetSpeed(_slowSpeed);
            _enemy.Animator.SetFloat("speedStateWalking", _slowSpeed);
            yield return new WaitForSeconds(_durationSlowSpeed);
            _isSlow = false;
        }

        _enemy.SetSpeedToDefault();
        _enemy.Animator.SetFloat("speedStateWalking", 1f);
    }

    public void StartBurning() {
        if (_immunityBurning) {
            return;
        }

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