using UnityEngine;

public class MeleeEnemy : Enemy
{
    [Header("Melee Settings")]
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private int meleeDamage = 5;
    [SerializeField] private EnemyHurtbox hurtbox;
    [SerializeField] private Transform attackDirection;
    [SerializeField] private float attackDuration;
    [SerializeField] private float attackRadius = 1.2f;
    [SerializeField] private float patrolRadius = 3f;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float chaseSpeed = 2.0f;
    [SerializeField] private LayerMask targetLayer;

    private bool isStunned = false;
    private float attackTimer = 0f;
    private bool isAttacking = false;
    protected override void Start()
    {
        base.Start();
        targetLayer = LayerMask.GetMask("Player");
    }
    protected override void Update()
    {
        base.Update();

        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;

            case EnemyState.Attack:
                attackTimer += Time.deltaTime;
                if (attackTimer >= attackCooldown)
                {
                    isAttacking = false;
                    attackTimer = 0f;

                    if (!IsTargetInRange(attackRadius))
                        SwitchState(EnemyState.Chase);
                }
                break;
        }
        Debug.Log("Current state is:" + currentState);
        Debug.Log("Current Layer is:" + targetLayer);
    }
    protected override void HandleIdle()
    {
        idleTimer += Time.deltaTime;
        if (idleTimer >= idleTime)
        {
            idleTimer = 0f;
            SwitchState(EnemyState.Patrol);
        }
    }
    protected override void HandleChase()
    {
        if (target == null || currentState != EnemyState.Chase) return;
        agent.speed = chaseSpeed;
        agent.SetDestination(target.transform.position);

        if (IsTargetInRange(attackRadius))
        {
            agent.ResetPath();
            SwitchState(EnemyState.Attack);
            Attack();
        }
        else if (!IsTargetInRange(detectionRadius))
        {
            SwitchState(EnemyState.Patrol);
        }
    }
    private void Attack()
    {
        if (isAttacking || currentState != EnemyState.Attack) return;

        isAttacking = true;

        //animator.setTrigger("attack");
        Debug.Log("Melee Enemy attacks!");
    }
    protected override void HandleStunned()
    {
        base.HandleStunned();
        agent.ResetPath();
        isAttacking = false;
    }
    private void Patrol()
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
        agent.speed = speed;
        if (newState == EnemyState.Patrol)
        {
            Patrol(); 
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
    private bool IsTargetInRange(float range)
    {
        if (target == null) return false;
        return Vector2.Distance(transform.position, target.transform.position) <= range;
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
