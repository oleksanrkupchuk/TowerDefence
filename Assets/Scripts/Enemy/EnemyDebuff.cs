using System.Collections;
using UnityEngine;

public class EnemyDebuff : MonoBehaviour {
    private bool _isSlow = false;
    private bool _isBurning = false;
    private bool _isPosion = false;

    [SerializeField]
    private Enemy _enemy;

    [Header("Debuff Slow")]
    public bool immunitySlow;
    [SerializeField]
    private float _slowSpeed;
    [SerializeField]
    private float _durationSlowSpeed;

    [Header("Debuff Burning")]
    public bool immunityBurning;
    [SerializeField]
    private float _durationBurning;

    [Header("Debuff Poison")]
    public bool immunityPoison;
    [SerializeField]
    private float _durationPoison;

    public void TakeDamage(float damage) {
        if (immunityBurning) {
            _enemy.StopBurningEffect();
        }
        else {
            _enemy.TakeDamage(damage);
        }
    }

    public void StartSlowMove() {
        if (immunitySlow) {
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
        if (immunityBurning) {
            return;
        }

        if (!_isBurning) {
            StartCoroutine(Burning());
        }
    }

    private IEnumerator Burning() {
        _isBurning = true;
        _enemy.PlayBurningEffect();
        Invoke(nameof(StopBurningAfterTime), _durationBurning);

        while (_isBurning) {
            yield return new WaitForSeconds(1.5f);
            TakeDamage(1.5f);
        }

        _enemy.StopBurningEffect();
    }

    private void StopBurningAfterTime() {
        _isBurning = false;
    }

    public void StartPoison() {
        if (immunityPoison) {
            return;
        }

        if (!_isPosion) {
            StartCoroutine(Poison());
        }
    }

    private IEnumerator Poison() {
        _isPosion = true;
        _enemy.PlayBurningEffect();

        Invoke(nameof(StopPosionAfterTime), _durationPoison);

        while (_isPosion) {
            yield return new WaitForSeconds(1f);
            TakeDamage(.5f);
        }

        _enemy.StopBurningEffect();
    }

    private void StopPosionAfterTime() {
        _isPosion = false;
    }

    public void StartImmunityBurninfForSomeTime() {
        immunityBurning = true;
        Invoke(nameof(StopImunityBurning), 3f);
    }

    private void StopImunityBurning() {
        immunityBurning = false;
    }
}
