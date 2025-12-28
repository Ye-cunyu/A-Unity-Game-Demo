using UnityEngine;

public class PlayerMoveState : PlayerGroundState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    : base(_player, _stateMachine, _animBoolName)
    {

    }
    public override void Update()
    {
        base.Update();

        if (xInput == 0)
        {
            stateMachine.ChangeState(player.idleState);

        }
    }
}
