public interface IState
{
    public void Enter(NPCMinotaur npc);

    public void Execute();

    public void OnEnterCollision();

    public void Exit();
}
