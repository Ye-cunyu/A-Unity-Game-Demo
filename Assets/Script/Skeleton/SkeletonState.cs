using UnityEngine;

public class SkeletonState
{
    protected SkeletonStateMachine stateMachine;//[访问修饰符] [数据类型] [变量名] [= 初始值];
    protected Skeleton skeleton;
    private string animBoolName;
    protected bool TriggerCalled;
    public SkeletonState(Skeleton _Skeleton, SkeletonStateMachine _stateMachine, string _animBoolName)
    {
        this.skeleton = _Skeleton;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }
    public virtual void Enter()
    {
        skeleton.anim.SetBool(animBoolName, true);
        TriggerCalled = false;



    }

    public virtual void Update()
    {
        if (skeleton.isKnockbacking)
        {
            return;
        }
        if (skeleton.IsDamaged)
        {
            return;
        }
        if (skeleton.IsDetectPlayer && stateMachine.currentState!=skeleton.attackedState)
        {
            stateMachine.ChangeState(skeleton.detectingState);
        }
        
    }
    public virtual void Exit()
    {
        skeleton.anim.SetBool(animBoolName, false);
    }
    public void AnimationFinishTrigger()
    {
        TriggerCalled = true;
    }
}
