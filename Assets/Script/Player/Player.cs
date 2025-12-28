using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : Entity
{
    public static Player instance;
    #region States
    public float stateTime;
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerFallState fallState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerAttackState attackState { get; private set; }
    public PlayerDamageState damageState { get; private set; }
    public PlayerDeadState deadState { get; private set; }
    #endregion
    public EntityStats stats { get; private set; }
    public event Action PlayerIsDead;

    public float flaskCooldown;
    public float flaskCooldownTimer;




    [Header("dash info")]
    public float dashTime;
    public float dashDuration;
    public float dashCooldownTimer;
    public float dashCooldown;

    



    protected override void Awake()
    {
        base.Awake();
        instance = this;
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "IsAir");
        fallState = new PlayerFallState(this, stateMachine, "IsAir");
        airState = new PlayerAirState(this, stateMachine, "IsAir");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "IsAir");
        attackState = new PlayerAttackState(this, stateMachine, "IsAttack");
        damageState = new PlayerDamageState(this, stateMachine, "Damaged");
        deadState = new PlayerDeadState(this, stateMachine, "Dead");
        stats = GetComponent<EntityStats>();
    }
    void Start()
    {
        stateMachine.Initialize(idleState);
    }


    public override void Update()
    {
        base.Update();
        if (isKnockbacking)
        {
            return;
        }
        if (IsDamaged||IsDead)
        {
            return;
        }
        stateMachine.currentState.Update();
        CollisionCheck();
        FlipController();


        dashTime -= Time.deltaTime;
        stateTime -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;
        flaskCooldownTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.F))
        {
            Inventory.instance.UseFlask();
        }


    }

    
    public void AnimationTrigger()
    {
        stateMachine.currentState.AnimationFinishTrigger();
    }
    private void FlipController()
    {
        if (rb.linearVelocity.x > minFlipSpeed && !faceRight)
        {
            Flip();
        }
        else if (rb.linearVelocity.x < -minFlipSpeed && faceRight)
        {
            Flip();
        }
    }
    public override void Damage(Vector2 attackDirection)
    {
        if (IsDead) return;
        base.Damage(attackDirection);
        if (stats.currentHealth <= 0)
        {
            IsDead = true;
            stateMachine.ChangeState(deadState);
        }
        else
        {
            IsDamaged = true;
            stateMachine.ChangeState(damageState);
        }
        fx.StartCoroutine("FlashFX");
    }

    public void TeleportPlayer(Vector3 position)
    {
        transform.position = position;
    }
    public void OnPlayerIsDead()
    {
        PlayerIsDead?.Invoke();
        stats.currentHealth = stats.maxHealth.GetValue();
        TriggerHealthChanged();
    }
}
