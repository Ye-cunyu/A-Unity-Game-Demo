using UnityEngine;

public class SkeletonDetectingState : SkeletonState
{
    private float flipDeadZone = 0.1f;
    float stopThreshold = 0.5f;
    public SkeletonDetectingState(Skeleton _skeleton, SkeletonStateMachine _stateMachine, string _animBoolName)
     : base(_skeleton, _stateMachine, _animBoolName)
    {

    }
    public override void Enter()
    {
        base.Enter();
        skeleton.stateTime = 3f;
    }
    public override void Update()
    {

        Vector2 direction = (skeleton.playerTarget.position - skeleton.transform.position).normalized;
        if (Mathf.Abs(direction.x) > flipDeadZone)
        {
            if (direction.x > 0 && !skeleton.faceRight)
            {
                skeleton.Flip();
            }
            else if (direction.x < 0 && skeleton.faceRight)
            {
                skeleton.Flip();
            }
        }
        if (Mathf.Abs(skeleton.playerTarget.position.x - skeleton.transform.position.x) <= stopThreshold)
        {
            skeleton.rb.linearVelocity = new Vector2(0, skeleton.rb.linearVelocityY);
        }
        else
        {
            skeleton.rb.linearVelocity = new Vector2(skeleton.faceDir * skeleton.moveSpeed * 1.5f, skeleton.rb.linearVelocityY);
        }
        if (!skeleton.IsDetectPlayer && skeleton.stateTime < 0)
        {
            skeleton.playerTarget = null;
            stateMachine.ChangeState(skeleton.moveState);
        }
        if (skeleton.ToAttack)
        {
            skeleton.ToAttack = false;
            stateMachine.ChangeState(skeleton.attackedState);
        }
    }
}
