using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }
    public override void Enter()
    {
        base.Enter();
        player.stateTime = 0.2f;
        player.rb.linearVelocity = new Vector2(6 * -player.faceDir, player.jumpForce);
    }
    public override void Update()
    {
        if (player.stateTime < 0)
        {
            stateMachine.ChangeState(player.airState);
        }
    }
}

