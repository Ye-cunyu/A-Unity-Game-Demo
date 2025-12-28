using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }
    private void DamageFinished()
    {
        player.DamageFinished();
    }
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        foreach (var hit in colliders)
        {
            if (hit.TryGetComponent(out Skeleton skeleton))
            {
                if (skeleton.IsDead)
                    continue;
                Vector2 attackDirection = (skeleton.transform.position - player.transform.position).normalized;
                skeleton.Damage(attackDirection);
                player.GetComponent<EntityStats>().DoDamage(skeleton.GetComponent<EntityStats>());
            }
        }
    }
    
}
