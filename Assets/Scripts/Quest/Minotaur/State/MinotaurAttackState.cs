using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurAttackState : IState {
    private NPCMinotaur _npc;

    public void Enter(NPCMinotaur npc) {
        _npc = npc;
        _npc.Animator.SetTrigger("attack");
    }

    public void Execute() {
        if(_npc.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1) {
            _npc.ChangeState(new MinotaurIdleState());
        }
    }

    public void Exit() {
        _npc.Animator.ResetTrigger("attack");
    }

    public void OnEnterCollision() {

    }
}
