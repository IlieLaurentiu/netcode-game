public class PlayerGroundedState : PlayerBaseState
{
    public override void EnterState(PlayerStateHandler player)
    {
        // Set initial gravity to the value of _gravityForce (given through the inspector in Unity)
        //player.initialGravityForce = player.gravityForce;
    }

    public override void UpdateState(PlayerStateHandler player)
    {

    }

    public override void ExitState(PlayerStateHandler player) { }
}