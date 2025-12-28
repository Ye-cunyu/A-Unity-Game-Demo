using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
   : base(_player, _stateMachine, _animBoolName)
    {

    }
    public override void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        player.anim.SetFloat("yVelocity", player.rb.linearVelocity.y);
        if (yInput >= 0)
        {
            player.rb.linearVelocity = new Vector2(player.rb.linearVelocity.x, player.rb.linearVelocity.y * .7f);
        }
        else
        {
            player.rb.linearVelocity = new Vector2(player.rb.linearVelocity.x, player.rb.linearVelocity.y);
        }
        if (player.stateTime < 0)
        {
            if (!player.IsWallSlided || !player.IsAir)
            {
                stateMachine.ChangeState(player.idleState);
                return;
            }

            if (xInput != 0 && xInput * player.faceDir < 0)
            {
                stateMachine.ChangeState(player.idleState);
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                stateMachine.ChangeState(player.wallJumpState);
            }
        }
        CheckForSpecialState();
    }
}