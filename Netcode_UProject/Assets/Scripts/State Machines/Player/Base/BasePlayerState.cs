public abstract class PlayerBaseState
{
    public abstract void EnterState(PlayerStateHandler player);

    public abstract void UpdateState(PlayerStateHandler player);

    public abstract void ExitState(PlayerStateHandler player);
}