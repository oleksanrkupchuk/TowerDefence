using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurIdleState : IState {
    private NPCMinotaur _npc;

    public void Enter(NPCMinotaur npc) {
        _npc = npc;
    }

    public void Execute() {
        if(_npc == null) {
            return;
        }

        if (_npc.move) {
            _npc.ChangeState(new MinotaurRunState());
        }

        if (_npc.Target != null && !_npc.Target.IsDead && _npc.onRoad) {
            _npc.ChangeState(new MinotaurRunState());
        }

        if(_npc.Target != null && _npc.Target.IsDead && _npc.onRoad) {
            _npc.SetTarget();
        }
    }

    public void Exit() {
        _npc.move = false;
    }

    public void OnEnterCollision() {
        throw new System.NotImplementedException();
    }
}
