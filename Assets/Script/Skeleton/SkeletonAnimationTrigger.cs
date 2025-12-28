using UnityEngine;

public class SkeletonAnimationTrigger : MonoBehaviour
{
    private Skeleton skeleton => GetComponentInParent<Skeleton>();

    private void AnimationTrigger()
    {
        skeleton.AnimationTrigger();
    }
    private void DamageFinished()
    {
        skeleton.DamageFinished();
    }
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(skeleton.attackCheck.position, skeleton.attackCheckRadius);
        foreach (var hit in colliders)
        {
            if (hit.TryGetComponent(out Player player))
            {
                Vector2 attackDirection = (player.transform.position - skeleton.transform.position).normalized;
                player.Damage(attackDirection);
                skeleton.GetComponent<EntityStats>().DoDamage(player.GetComponent<EntityStats>());
            }
        }
    }
}

