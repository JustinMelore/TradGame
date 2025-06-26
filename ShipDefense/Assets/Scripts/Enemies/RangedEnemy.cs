using UnityEngine;

public class RangedEnemy : Enemy
{
    [Header("Ranged Settings")]
    [SerializeField] private GameObject projectile;
    [SerializeField] protected LayerMask targetLayer;
    [SerializeField] protected float detectionRadius = 12.0f;
    [SerializeField] protected float beginEvadingRadius = 6.0f;
    [SerializeField] protected float patrolRadius = 4.0f;
    [SerializeField] protected float chaseSpeed = 2.0f;

    private float currentFireTime;
    private Vector3 currentEvadeDirection;
    protected override void Start()
    {
        base.Start();
        lastAttackTime = Time.time;
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

            case EnemyState.Evading:
                HandleEvading();
                break;
        }

        //if (target != null && Time.time - lastAttackTime >= attackCooldown)
        //{
        //    FireProjectile();
        //    lastAttackTime = Time.time;
        //}
    }


    protected override void HandleChase()
    {
        if (target == null || currentState != EnemyState.Chase) return;
        agent.speed = chaseSpeed;
        agent.SetDestination(target.transform.position);
        FireProjectile();
        if (DetectTargetInRadius(beginEvadingRadius))
        {
            agent.ResetPath();
            SwitchState(EnemyState.Evading);
        }
        else if (!DetectTargetInRadius(detectionRadius))
        {
            SwitchState(EnemyState.Patrol);
        }
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
    private void HandleEvading()
    {
        if (target == null || currentState != EnemyState.Evading) return;
        agent.speed = chaseSpeed;
        if(currentEvadeDirection == null || agent.remainingDistance < 0.2f)
        {
            Vector2 randomDir = Random.insideUnitCircle * beginEvadingRadius;
            Vector3 patrolTarget = transform.position + new Vector3(randomDir.x, randomDir.y, 0f);
            agent.SetDestination(patrolTarget);
        }
        FireProjectile();
        if (!DetectTargetInRadius(beginEvadingRadius))
        {
            SwitchState(EnemyState.Chase);
        }
    }

    protected override void SwitchState(EnemyState newState)
    {
        base.SwitchState(newState);
        if (agent != null) agent.speed = speed;
        if (newState == EnemyState.Patrol)
        {
            HandlePatrol();
        } else if (newState == EnemyState.Evading)
        {
            HandleEvading();
        }
        Debug.Log("Current state: " + currentState);
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

    private void FireProjectile()
    {
        if (target == null || Time.time - lastAttackTime < attackCooldown) return;

        Vector3 direction = (target.transform.position - transform.position).normalized;
        Vector3 spawnOffset = direction * 0.5f;

        Vector3 projectileSpawnPosition = transform.position + spawnOffset;
        Projectile firedProjectile = Instantiate(projectile, projectileSpawnPosition, Quaternion.identity).GetComponent<Projectile>();
        firedProjectile.ChangeMoveDirection(direction);
        lastAttackTime = Time.time;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, beginEvadingRadius);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
    }
}
