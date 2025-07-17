using System.Collections;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    [Header("Melee Settings")]
    ////[SerializeField] private float attackRange = 1.5f;
    [SerializeField] protected int meleeDamage = 5;
    [SerializeField] protected EnemyHurtbox hurtbox;
    [SerializeField] protected Transform attackDirection;
    [SerializeField] protected float attackDuration = 1.4f;
    [SerializeField] protected float attackRadius = 1.0f;
    [SerializeField] protected float patrolRadius = 4.0f;
    [SerializeField] protected float detectionRadius = 6.0f;
    [SerializeField] protected float chaseSpeed = 2.0f;
    [SerializeField] protected LayerMask targetLayer;
    [SerializeField] protected GameObject attackWarningSign;
    [SerializeField] protected GameObject stunSign;

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
        if (stop)
        {
            DisableStun();
            DisableAttackWarning();
            hurtbox.Deactivate();
        }
        if (currentState == EnemyState.Dead) {
            DisableStun();
            DisableAttackWarning();
            hurtbox.Deactivate();
        }
        base.Update();
        targetLayer = LayerMask.GetMask("Player");
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
                UpdateAttackDirection();
                HandleAttack();
                break;
        }
        //Debug.Log("TargetLayer is" + targetLayer);
        //Debug.Log("Target is" + target);
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
        Vector2 dirToTarget = (target.transform.position - transform.position).normalized;
        if (dirToTarget.x != 0)
        {
            animator.SetFloat("RunX", dirToTarget.x > 0 ? 1f : -1f);
        }
        if (DetectTargetInRadius(attackRadius) && Time.time - lastAttackTime >= attackCooldown)
        {
            agent.ResetPath();
            SwitchState(EnemyState.Attack);
        }
        else if (!DetectTargetInRadius(detectionRadius))
        {
           SwitchState(EnemyState.Patrol);
        }
    }
    protected virtual void HandleAttack()
    {
        //if (!isAttacking)
        //{
        //    isAttacking = true;
        //    animator.SetBool("Attacking", true);
        //}
        //Debug.Log("Melee Enemy attacks!");
        if (!isAttacking && Time.time - lastAttackTime >= attackCooldown)
        {
            isAttacking = true;
            lastAttackTime = Time.time;
            animator.SetBool("Attacking", true);
        }
    }
    public virtual void Attack()
    {
        if (currentState == EnemyState.Stunned) return;
        Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
        float attackOffset = 1.2f;
        attackDirection.position = transform.position + directionToTarget * attackOffset;
        float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
        attackDirection.rotation = Quaternion.Euler(0f, 0f, angle);
        float x = directionToTarget.x;
        animator.SetFloat("AttackX", x >= 0 ? 1f : 0f);
        hurtbox.transform.position = attackDirection.position;
        hurtbox.transform.rotation = attackDirection.rotation;
        hurtbox.Activate(meleeDamage);
    }
    public void UpdateAttackDirection()
    {
        if (currentState == EnemyState.Stunned) return;
        Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
        float attackOffset = 1.2f;
        attackDirection.position = transform.position + directionToTarget * attackOffset;
        float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
        attackDirection.rotation = Quaternion.Euler(0f, 0f, angle);
        float x = directionToTarget.x;
        animator.SetFloat("AttackX", x >= 0 ? 1f : 0f);
    }
    public void CanParry()
    {
        canParry = true;
    }
    public void CannotParry()
    {
        canParry = false;
    }
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

    public void AttackWarning()
    {
        attackWarningSign.SetActive(true);
    }
    public void DisableAttackWarning()
    {
        attackWarningSign.SetActive(false);
    }
    public void ShowStun()
    {
        stunSign.SetActive(true);
    }
    public void DisableStun()
    {
        stunSign.SetActive(false);
    }
    protected override void HandlePatrol()
    {
        if (agent != null && !agent.hasPath || agent.remainingDistance < 0.2f)
        {
            agent.ResetPath();
            Vector2 randomDir = Random.insideUnitCircle * patrolRadius;
            Vector3 patrolTarget = transform.position + new Vector3(randomDir.x, randomDir.y, 0f);
            agent.SetDestination(patrolTarget);
            float directionX = patrolTarget.x - transform.position.x;
            if (Mathf.Abs(directionX) > 0.1f) 
            {
                animator.SetFloat("RunX", directionX > 0 ? 1f : -1f);
            }
        }
        if (DetectTargetInRadius(detectionRadius))
        {
            SwitchState(EnemyState.Chase);
        }
    }
    protected override void SwitchState(EnemyState newState)
    {
        base.SwitchState(newState);
        hurtbox.Deactivate();
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
                target = hit.gameObject;
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
