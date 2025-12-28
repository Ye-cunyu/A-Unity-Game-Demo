using UnityEngine;

public class SkeletonDamageState : SkeletonState
{
    public SkeletonDamageState(Skeleton _skeleton, SkeletonStateMachine _stateMachine, string _animBoolName)
    : base(_skeleton, _stateMachine, _animBoolName)
    {

    }
    public override void Enter()
    {
        base.Enter();

    }
    public override void Update()
    {
        base.Update();
        if (!skeleton.IsDamaged && !skeleton.isKnockbacking) 
        {
            stateMachine.ChangeState(skeleton.idleState);
        }
    }
}
