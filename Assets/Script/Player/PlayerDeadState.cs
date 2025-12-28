using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
   : base(_player, _stateMachine, _animBoolName)
    {

    }
    public override void Enter()
    {
        base.Enter();
        player.StartCoroutine(DeadDelay());
    }
    public override void Exit()
    {
        base.Exit();
    }

    private IEnumerator DeadDelay()
    {
        yield return new WaitForSeconds(5f);
        Debug.Log("revival");
        player.OnPlayerIsDead();
        stateMachine.ChangeState(player.idleState);
        player.IsDead = false;
    }
}
