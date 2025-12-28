using UnityEngine;

public class PlayerAttackState : PlayerState
{
    public PlayerAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
   : base(_player, _stateMachine, _animBoolName)
    {

    }
    private int ComboCounter;
    private float LastTimeAttacked;
    private float comboWindow = 3;
    public override void Enter()
    {
        base.Enter();
        if (ComboCounter > 2 || comboWindow + LastTimeAttacked <= Time.time)
        {
            ComboCounter = 0;
        }
        player.anim.SetInteger("ComboCounter", ComboCounter);
    }
    public override void Update()
    {
        base.Update();
        player.rb.linearVelocity = new Vector2(0, 0);
        if (TriggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
    public override void Exit()
    {
        base.Exit();
        ComboCounter++;
        LastTimeAttacked = Time.time;
    }
}
