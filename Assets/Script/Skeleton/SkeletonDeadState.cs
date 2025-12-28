using System.Collections;
using UnityEngine;

public class SkeletonDeadState : SkeletonState
{
    public SkeletonDeadState(Skeleton _skeleton, SkeletonStateMachine _stateMachine, string _animBoolName)
     : base(_skeleton, _stateMachine, _animBoolName)
    {

    }
    public override void Enter()
    {
        base.Enter();
        skeleton.SetVelocity(0, skeleton.rb.linearVelocityY);
        //skeleton.GetComponent<Collider2D>().enabled = false;
        skeleton.StartCoroutine(DestroyAfterDelay());
    }
    public override void Update()
    {

    }
    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(2.5f);
        Object.Destroy(skeleton.gameObject);
    }
}
