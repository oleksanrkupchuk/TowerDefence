using UnityEngine;

public class IronBullet : Bullet {
    public bool thonr;

    private new void Start() {
        base.Start();
    }

    private new void Update() {
        base.Update();
    }

    protected override void CalculationT() {
        _previousPosition = Bezier.GetTrajectoryForBullet(_bezierPoints[0].transform.position,
                       _bezierPoints[1].transform.position, _bezierPoints[2].transform.position, _t - 0.1f);

        _timeFormula1 += Time.deltaTime;

        if (transform.position.y > _previousPosition.y) {
            _timeFormulaBuffer = 1 / (1 + _timeFormula1) * _timeFormula1 * 1.5f;
            //_timeFormulaBuffer = 1 / (1 + _timeFormula1) * _timeFormula1;
            _t = _timeFormulaBuffer;
        }
        else {
            _timeFormula2 += Time.deltaTime;
            //_t = _timeFormulaBuffer + _timeFormula2;
            //_t = _timeFormulaBuffer + (_timeFormula2 * _timeFormula2 * 1.5f);
            //_t = _timeFormulaBuffer + (_timeFormula2 * _timeFormula2 * 2.5f);
            _t = _timeFormulaBuffer + (_timeFormula2 + _timeFormula2 * _timeFormula2);
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.TryGetComponent(out Enemy enemy)) {
            if (_target == enemy) {
                _tower.RemoveBullet(this);
                _target.LastPosition -= SetTargetPosition;
                _target.TakeDamage(_damage);
                ChecBuyAbilityAndSlowEnemy(_target);
                SetTargetPositionAndSetTargetNull();
            }
        }
    }

    private void ChecBuyAbilityAndSlowEnemy(Enemy _enemy) {
        if (thonr) {
            _enemy.Debuff.StartSlowMove();
        }
    }
}
