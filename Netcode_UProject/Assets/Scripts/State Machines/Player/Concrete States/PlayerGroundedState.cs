public class PlayerGroundedState : PlayerBaseState
{
    public override void EnterState(PlayerStateHandler player)
    {
       //  Set initial gravity to the value of _gravityForce (given through the inspector in Unity)
        player.initialGravityForce = player.gravityForce;
        player.gravityForce = 0;
    }

    public override void UpdateState(PlayerStateHandler player)
    {
        // Multiplying by Time.fixedDeltaTime here is unnecessary, because velocity is distance/time. The Rigidbody uses Time.deltaTime internally already
        player.playerRigidBody.velocity = -player.transform.forward * player.horizontalInput + player.transform.right * player.verticalInput + player.transform.up * player.gravityForce;
    }

    public override void ExitState(PlayerStateHandler player) 
    {
        player.gravityForce = player.initialGravityForce;
    }
}