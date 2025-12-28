using UnityEngine;

public class SkeletonStats : EntityStats
{
    protected Skeleton skeleton;
    public override void Awake()
    {
        base.Awake();
        skeleton = GetComponent<Skeleton>();
    }
    public override void Update()
    {
        base.Update();
        if (skeleton.IsDead)
        {
            return;
        }
        if (currentHealth<=0)
        {
            skeleton.stateMachine.ChangeState(skeleton.deadState);
            skeleton.IsDead = true;
        }
    }
}
