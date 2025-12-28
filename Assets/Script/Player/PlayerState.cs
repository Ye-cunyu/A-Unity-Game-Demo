using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;//[访问修饰符] [数据类型] [变量名] [= 初始值];
    protected Player player;
    private string animBoolName;

    protected float xInput;
    protected float yInput;
    protected bool TriggerCalled;
    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }
    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
        TriggerCalled = false;

        

    }

    public virtual void Update()
    {
        if (player.isKnockbacking)
        {
            return;
        }
        if (player.IsDamaged)
        {
            return;
        }
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        player.anim.SetFloat("yVelocity", player.rb.linearVelocityY);
        player.SetVelocity(xInput * player.moveSpeed, player.rb.linearVelocityY);
        CheckForSpecialState();
    }
    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }
    public void CheckForSpecialState()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && player.dashCooldownTimer < 0)
        {
            stateMachine.ChangeState(player.dashState);
        }
        if (player.IsWallSlided && player.IsAir && stateMachine.currentState != player.wallSlideState && stateMachine.currentState != player.wallJumpState)
        {
            stateMachine.ChangeState(player.wallSlideState);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            stateMachine.ChangeState(player.attackState);
        }
    }
    public void AnimationFinishTrigger()
    {
        TriggerCalled = true;
    }
}