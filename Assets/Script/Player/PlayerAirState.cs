using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
   : base(_player, _stateMachine, _animBoolName)
    {

    }
    public override void Update()
    {
        base.Update();
        if (!player.IsAir)
        {
            stateMachine.ChangeState(player.idleState);
        }
        
    }
    
}
