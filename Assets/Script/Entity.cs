using System;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }

    #endregion
    #region info

    [Header("collision info")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] public Transform attackCheck;
    [SerializeField] public float attackCheckRadius;
    

    [Header("flip info")]
    public int faceDir = 1;
    public bool faceRight = true;
    public float minFlipSpeed = 0.1f;
    public event Action onFlipped;
    [Header("Move info")]
    public float moveSpeed;
    public float jumpForce;
    public bool IsAir;
    public bool IsWallSlided;
    #endregion

    [Header("Knockback Settings")]
    [SerializeField] protected float knockbackForce = 8f;
    [SerializeField] protected float knockbackDuration = 0.3f;
    public bool isKnockbacking;
    protected float knockbackTimer;
    protected Vector2 knockbackDirection;
    public bool IsDamaged;
    public bool IsDead;
    public event Action onHealthChanged;



    public EntityFX fx { get; private set; }


    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        fx = GetComponent<EntityFX>();
    }
    public virtual void Update()
    {
        if (isKnockbacking)
        {
            HandleKnockback();
        }
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(new Vector2(faceDir, 0) * wallCheckDistance));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    protected virtual void CollisionCheck()
    {
        IsAir = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround).collider == null;
        IsWallSlided = Physics2D.Raycast(wallCheck.position, new Vector2(faceDir, 0), wallCheckDistance, whatIsGround).collider != null;
    }
    public void Flip()
    {
        faceDir *= -1;
        faceRight = !faceRight;
        transform.Rotate(0, 180, 0);
        onFlipped?.Invoke();
    }

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        rb.linearVelocity = new Vector2(_xVelocity, _yVelocity);
    }
    public void SetVelocity(Vector2 velocity)
    {
        rb.linearVelocity = velocity;
    }
    protected virtual void HandleKnockback()
    {
        if (isKnockbacking)
        {
            knockbackTimer -= Time.deltaTime;

            float forceMultiplier = knockbackTimer / knockbackDuration;
            SetVelocity(knockbackForce * knockbackDirection * forceMultiplier);

            if (knockbackTimer < 0)
            {
                isKnockbacking = false;
                SetVelocity(0, 0);
            }
        }
    }
    public virtual void Damage(Vector2 attackDirection)
    {
        onHealthChanged?.Invoke();
        isKnockbacking = true;
        knockbackTimer = knockbackDuration;
        knockbackDirection = attackDirection.normalized;
        
        if (knockbackDirection.y > -0.3f)
        {
            knockbackDirection.y = 0.5f;
        }
    }
    public virtual void ResetKnockback()
    {
        isKnockbacking = false;
        knockbackTimer = 0;
        SetVelocity(0, 0);
    }
    public void DamageFinished()
    {
        IsDamaged = false;
        ResetKnockback();

    }
    public void TriggerHealthChanged()
    {
        onHealthChanged?.Invoke();
    }
}
