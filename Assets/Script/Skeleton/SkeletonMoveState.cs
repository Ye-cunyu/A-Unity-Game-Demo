using UnityEngine;

public class SkeletonMoveState : SkeletonGroundState
{
    public SkeletonMoveState(Skeleton _skeleton, SkeletonStateMachine _stateMachine, string _animBoolName)
   : base(_skeleton, _stateMachine, _animBoolName)
    {

    }
    public override void Update()
    {
        base.Update();
        skeleton.rb.linearVelocity = new Vector2(skeleton.faceDir*skeleton.moveSpeed,skeleton.rb.linearVelocityY);
        if (skeleton.ToFlip ||skeleton.IsWallSlided)
        {
            skeleton.Flip();
            stateMachine.ChangeState(skeleton.idleState);
        }
    }
}
