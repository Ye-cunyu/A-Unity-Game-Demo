using UnityEngine;

public class PlayerDamageState : PlayerState
{
    public PlayerDamageState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
   : base(_player, _stateMachine, _animBoolName)
    {

    }
    public override void Update()
    {
        base.Update();
        if (!player.IsDamaged && !player.isKnockbacking)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
