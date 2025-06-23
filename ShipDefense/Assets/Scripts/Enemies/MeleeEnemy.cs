using UnityEngine;

public class MeleeEnemy : Enemy
{
    [Header("Melee Settings")]
    ////[SerializeField] private float attackRange = 1.5f;
    [SerializeField] protected int meleeDamage = 5;
    [SerializeField] protected EnemyHurtbox hurtbox;
    [SerializeField] protected Transform attackDirection;
    [SerializeField] protected float attackDuration;
    [SerializeField] protected float attackRadius = 1.0f;
    [SerializeField] protected float patrolRadius = 4.0f;
    [SerializeField] protected float detectionRadius = 6.0f;
    [SerializeField] protected float chaseSpeed = 2.0f;
    [SerializeField] protected LayerMask targetLayer;

    private float attackTimer = 0f;
    protected bool isAttacking = false;
    public bool canParry = false;
    protected override void Start()
    {
        base.Start();
        targetLayer = LayerMask.GetMask("Player");
    }
    protected override void Update()
    {
        base.Update();

        float currentSpeed = agent.velocity.magnitude;
        animator.SetFloat("Speed", currentSpeed);
        animator.SetBool("HasTarget", DetectTargetInRadius(detectionRadius));

        switch (currentState)
        {
            case EnemyState.Patrol:
                HandlePatrol();
                break;

            case EnemyState.Chase:
                HandleChase();
                break;

            case EnemyState.Attack:
                HandleAttack();
                break;
        }
        //Debug.Log("Current state is:" + currentState);
        //Debug.Log("Current Layer is:" + targetLayer);
    }
    //protected override void HandleIdle()
    //{
    //    idleTimer += Time.deltaTime;
    //    if (idleTimer >= idleTime)
    //    {
    //        idleTimer = 0f;
    //        HandlePatrol();
    //    }
    //}
    protected override void HandleChase()
    {
        if (target == null || currentState != EnemyState.Chase) return;
        agent.speed = chaseSpeed;
        agent.SetDestination(target.transform.position);

        if (DetectTargetInRadius(attackRadius))
        {
            agent.ResetPath();
            SwitchState(EnemyState.Attack);
        }
        else if (!DetectTargetInRadius(detectionRadius))
        {
           SwitchState(EnemyState.Patrol);
        }
    }
    protected void HandleAttack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            animator.SetBool("Attacking", true);
        }
        Debug.Log("Melee Enemy attacks!");
    }
    public virtual void Attack()
    {
        if (currentState == EnemyState.Stunned) return;
        Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
        float attackOffset = 0.6f;
        attackDirection.position = transform.position + directionToTarget * attackOffset;
        float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
        attackDirection.rotation = Quaternion.Euler(0f, 0f, angle);
        hurtbox.transform.position = attackDirection.position;
        hurtbox.transform.rotation = attackDirection.rotation;
        hurtbox.Activate(meleeDamage);
    }
    public void CanParry()
    {
        canParry = true;
    }
    //
    public virtual void OnAttackEnd()
    {
        isAttacking = false;
        animator.SetBool("Attacking", false);
        animator.ResetTrigger("Attack");
        if (DetectTargetInRadius(attackRadius) && currentState != EnemyState.Stunned)
        {
            SwitchState(EnemyState.Chase);
        }
        else if (DetectTargetInRadius(detectionRadius) && currentState != EnemyState.Stunned)
        {
            SwitchState(EnemyState.Chase);
        }
        else
        {
            SwitchState(EnemyState.Patrol);
        }
        canParry = false;
        hurtbox.Deactivate();
    }
    private void HandlePatrol()
    {
        if (!agent.hasPath || agent.remainingDistance < 0.2f)
        {
            Vector2 randomDir = Random.insideUnitCircle * patrolRadius;
            Vector3 patrolTarget = transform.position + new Vector3(randomDir.x, randomDir.y, 0f);
            agent.SetDestination(patrolTarget);
        }
        if (DetectTargetInRadius(detectionRadius))
        {
            SwitchState(EnemyState.Chase);
        }
    }
    protected override void SwitchState(EnemyState newState)
    {
        base.SwitchState(newState);
        if(agent != null) agent.speed = speed;
        if (newState == EnemyState.Patrol)
        {
            HandlePatrol(); 
        }
    }
    private bool DetectTargetInRadius(float radius)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
    }
}
