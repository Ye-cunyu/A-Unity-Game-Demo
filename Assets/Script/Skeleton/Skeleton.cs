using UnityEngine;


public class Skeleton : Entity
{

    public float stateTime;
    public SkeletonStateMachine stateMachine { get; private set; }
    public SkeletonIdleState idleState { get; private set; }
    public SkeletonMoveState moveState { get; private set; }
    public SkeletonDetectingState detectingState { get; private set; }
    public SkeletonAttackedState attackedState { get; private set; }
    public SkeletonDamageState damageState { get; private set; }
    public SkeletonDeadState deadState{ get; private set; }
    public EntityStats stats { get; private set; }
    [SerializeField] protected LayerMask whatIsPlayer;

    public bool ToFlip;
    [SerializeField] protected Transform flipCheck;
    [SerializeField] protected float flipCheckDistance;
    [SerializeField] private float detectionRadius;
    public bool IsDetectPlayer;
    public bool ToAttack;
    
    
    public Transform playerTarget;
    [SerializeField] protected Transform AttackCheckLine;
    [SerializeField] protected float AttackCheckDistance;


    protected override void Awake()
    {
        base.Awake();
        stateMachine = new SkeletonStateMachine();
        idleState = new SkeletonIdleState(this, stateMachine, "Idle");
        moveState = new SkeletonMoveState(this, stateMachine, "Move");
        detectingState = new SkeletonDetectingState(this, stateMachine, "Move");
        attackedState = new SkeletonAttackedState(this, stateMachine, "Attack");
        damageState = new SkeletonDamageState(this, stateMachine, "Damage");
        deadState = new SkeletonDeadState(this, stateMachine, "Dead");
        stats = GetComponent<EntityStats>();
    }
    private void OnValidate()
    {
        gameObject.name = "Skeleton";
    }
    void Start()
    {
        stateMachine.Initialize(idleState);

    }
    public override void Update()
    {
        base.Update();
        if (isKnockbacking || IsDamaged || IsDead)
        {
            return;
        }
        
        stateTime -= Time.deltaTime;
        stateMachine.currentState.Update();
        CollisionCheck();
        CheckForPlayer();
    }
    protected override void CollisionCheck()
    {
        base.CollisionCheck();
        ToFlip = Physics2D.Raycast(flipCheck.position, Vector2.down, flipCheckDistance, whatIsGround).collider == null;
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(flipCheck.position, new Vector3(flipCheck.position.x, flipCheck.position.y - flipCheckDistance));

        Gizmos.color = IsDetectPlayer ? Color.red : Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = ToAttack ? Color.red : Color.yellow;
        Gizmos.DrawLine(AttackCheckLine.position, AttackCheckLine.position + Vector3.right * faceDir * AttackCheckDistance);

    }
    private void CheckForPlayer()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRadius, whatIsPlayer);
        if (playerCollider != null)
        {
            playerTarget = playerCollider.transform;
            IsDetectPlayer = true;
        }
        else
        {
            IsDetectPlayer = false;
        }

        ToAttack = Physics2D.Raycast(AttackCheckLine.position, Vector2.right * faceDir, AttackCheckDistance, whatIsPlayer).collider != null;
    }
    public void AnimationTrigger()
    {
        stateMachine.currentState.AnimationFinishTrigger();
    }
    public override void Damage(Vector2 attackDirection)
    {
        base.Damage(attackDirection);
        IsDamaged = true;
        stateMachine.ChangeState(damageState);
        fx.StartCoroutine("FlashFX");
    }
    
}

    

