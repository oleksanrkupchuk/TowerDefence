public class MinotaurIdleState : MinotaurState {

    public override void Execute() {
        if(_npcMinotaur == null) {
            return;
        }

        if (_npcMinotaur.move) {
            _npcMinotaur.ChangeState(new MinotaurRunState());
        }

        if (_npcMinotaur.Target != null && !_npcMinotaur.Target.IsDead && _npcMinotaur.onRoad) {
            _npcMinotaur.ChangeState(new MinotaurRunState());
        }

        if(_npcMinotaur.Target != null && _npcMinotaur.Target.IsDead && _npcMinotaur.onRoad) {
            _npcMinotaur.SetTarget();
        }
    }

    public override void Exit() {
        _npcMinotaur.move = false;
    }
}
