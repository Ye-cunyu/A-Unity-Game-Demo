using UnityEngine;

public class SkeletonAttackedState : SkeletonState
{
    public SkeletonAttackedState(Skeleton _skeleton, SkeletonStateMachine _stateMachine, string _animBoolName)
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
        skeleton.rb.linearVelocity = new Vector2(0, skeleton.rb.linearVelocityY);
        if (TriggerCalled)
        {
            stateMachine.ChangeState(skeleton.detectingState);
        }
    }
}
