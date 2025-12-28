using UnityEngine;

public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
     : base(_player, _stateMachine, _animBoolName)
    {

    }
    public override void Enter()
    {
        base.Enter();
        player.rb.linearVelocity = new Vector2(player.rb.linearVelocity.x, player.jumpForce);
    }

}
