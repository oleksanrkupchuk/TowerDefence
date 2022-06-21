public abstract class MinotaurState
{
    protected NPCMinotaur _npcMinotaur;
    public virtual void Enter(NPCMinotaur npcMinotaur) {
        _npcMinotaur = npcMinotaur;
    }

    public virtual void Execute() { }

    public virtual void OnEnterCollision() { }

    public virtual void Exit() { }
}
