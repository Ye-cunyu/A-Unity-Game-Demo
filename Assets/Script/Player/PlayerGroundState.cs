using UnityEngine;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
   : base(_player, _stateMachine, _animBoolName)
    {

    }
    public override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.jumpState);
        }
        if (player.IsAir)
        {
            stateMachine.ChangeState(player.airState);
        }
    }
}
