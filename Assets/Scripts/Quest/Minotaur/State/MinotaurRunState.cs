using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurRunState : IState {
    private NPCMinotaur _npc;

    private int _indexPosition;

    public void Enter(NPCMinotaur npc) {
        _npc = npc;
        _npc.Animator.SetBool("run", true);
    }

    public void Execute() {
        if (_npc == null) {
            return;
        }

        if (_npc.runWayPoint) {
            GetNextPosition();
            Move(_npc.nextWayPoint);
            CheckFlipSprite(_npc.nextWayPoint);
        }

        if (!_npc.runWayPoint) {
            if (_npc.Target != null && !_npc.Target.IsDead) {
                _npc.SetLayer();
                CheckFlipSprite(_npc.Target.transform);
                CheckDistanceToTargetAndAttackOrMove();
            }
            else {
                _npc.ChangeState(new MinotaurIdleState());
            }
        }
    }

    private void GetNextPosition() {
        if (Vector2.Distance(_npc.transform.position, _npc.nextWayPoint.position) <= 0.02f) {

            _indexPosition++;
            if (_indexPosition < _npc.WayPoints.Count) {
                _npc.nextWayPoint = _npc.WayPoints[_indexPosition];
            }
            else {
                _npc.InitStartPosition();
                _npc.runWayPoint = false;
                _npc.onRoad = true;
                _npc.ChangeState(new MinotaurIdleState());
            }
        }
    }

    private void Move(Transform target) {
        _npc.transform.position = Vector2.MoveTowards(_npc.transform.position, target.position, _npc.speed * Time.deltaTime);
    }

    private void CheckFlipSprite(Transform target) {
        if (_npc.transform.position.x - target.position.x < 0 && _npc.isFlipLeft) {
            FlipSprite();
        }

        if (_npc.transform.position.x - target.position.x > 0 && !_npc.isFlipLeft) {
            FlipSprite();
        }
    }

    private void FlipSprite() {
        _npc.isFlipLeft = !_npc.isFlipLeft;
        _npc.transform.localScale = new Vector3(_npc.transform.localScale.x * -1, _npc.transform.localScale.y);
    }

    private void CheckDistanceToTargetAndAttackOrMove() {
        if (Vector2.Distance(_npc.transform.position, _npc.Target.transform.position) <= 0.5f) {
            _npc.Target.isUnderAttack = true;
            _npc.Target.CheckFlipSprite(_npc.transform);
            _npc.ChangeState(new MinotaurAttackState());
        }
        else {
            Move(_npc.Target.transform);
        }
    }

    public void Exit() {
        _npc.Animator.SetBool("run", false);
    }

    public void OnEnterCollision() {
        throw new System.NotImplementedException();
    }
}
