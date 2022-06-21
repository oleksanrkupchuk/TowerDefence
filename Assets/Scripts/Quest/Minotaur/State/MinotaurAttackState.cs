public class MinotaurAttackState : MinotaurState {

    public override void Enter(NPCMinotaur npcMinotaur) {
        _npcMinotaur = npcMinotaur;
        _npcMinotaur.Animator.SetTrigger("attack");
    }

    public override void Execute() {
        if(_npcMinotaur.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1) {
            _npcMinotaur.ChangeState(new MinotaurIdleState());
        }
    }

    public override void Exit() {
        _npcMinotaur.Animator.ResetTrigger("attack");
    }
}
