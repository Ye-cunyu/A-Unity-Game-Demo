using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    : base(_player, _stateMachine, _animBoolName)
    {

    }
    public override void Enter()
    {
        base.Enter();
        player.dashTime = player.dashDuration;
        player.dashCooldownTimer = player.dashCooldown;
    }
    public override void Update()
    {
        base.Update();
        player.rb.linearVelocity = new Vector2(player.faceDir * player.moveSpeed * 2.5f, 0);
        if (player.dashTime < 0)
        {
            stateMachine.ChangeState(player.moveState);
        }

    }
}
