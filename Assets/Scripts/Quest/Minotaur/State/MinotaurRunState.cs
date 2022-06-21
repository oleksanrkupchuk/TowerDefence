using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurRunState : MinotaurState {
    private int _indexPosition;

    public override void Enter(NPCMinotaur npcMinotaur) {
        _npcMinotaur = npcMinotaur;
        _npcMinotaur.Animator.SetBool("run", true);
    }

    public override void Execute() {
        if (_npcMinotaur == null) {
            return;
        }

        if (_npcMinotaur.runWayPoint) {
            GetNextPosition();
            _npcMinotaur.Move(_npcMinotaur.nextWayPoint);
            _npcMinotaur.CheckFlipSprite(_npcMinotaur.nextWayPoint);
        }

        if (!_npcMinotaur.runWayPoint) {
            if (_npcMinotaur.Target != null && !_npcMinotaur.Target.IsDead) {
                _npcMinotaur.SetLayer();
                _npcMinotaur.CheckFlipSprite(_npcMinotaur.Target.transform);
                CheckDistanceToTargetAndAttackOrMove();
            }
            else {
                _npcMinotaur.ChangeState(new MinotaurIdleState());
            }
        }
    }

    private void GetNextPosition() {
        if (Vector2.Distance(_npcMinotaur.transform.position, _npcMinotaur.nextWayPoint.position) <= 0.02f) {

            _indexPosition++;
            if (_indexPosition < _npcMinotaur.WayPoints.Count) {
                _npcMinotaur.nextWayPoint = _npcMinotaur.WayPoints[_indexPosition];
            }
            else {
                _npcMinotaur.InitStartPosition();
                _npcMinotaur.runWayPoint = false;
                _npcMinotaur.onRoad = true;
                _npcMinotaur.ChangeState(new MinotaurIdleState());
            }
        }
    }

    private void CheckDistanceToTargetAndAttackOrMove() {
        if (Vector2.Distance(_npcMinotaur.transform.position, _npcMinotaur.Target.transform.position) <= 0.5f) {
            _npcMinotaur.Target.isUnderAttack = true;
            _npcMinotaur.Target.CheckFlipSprite(_npcMinotaur.transform);
            _npcMinotaur.ChangeState(new MinotaurAttackState());
        }
        else {
            _npcMinotaur.Move(_npcMinotaur.Target.transform);
        }
    }

    public override void Exit() {
        _npcMinotaur.Animator.SetBool("run", false);
    }
}
