using UnityEngine;

public class SkeletonIdleState : SkeletonGroundState
{

    public SkeletonIdleState(Skeleton _skeleton, SkeletonStateMachine _stateMachine, string _animBoolName)
   : base(_skeleton, _stateMachine, _animBoolName)
    {

    }
    public override void Enter()
    {
        base.Enter();
        skeleton.stateTime = 1f;
        skeleton.rb.linearVelocity = new Vector2(0,0);
    }
    public override void Update()
    {
        base.Update();
        if (skeleton.stateTime < 0)
        {
            stateMachine.ChangeState(skeleton.moveState);
            
        }
    }
}
