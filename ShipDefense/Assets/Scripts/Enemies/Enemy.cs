using System.Collections;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Script to define enemy behavior. This script is mostly for testing purposes and will later be exchanged with more specific scripts for different enemy types.
/// </summary>
public class Enemy : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D enemy;
    protected WaveSpawner waveSpawner;

    [Header("Common Enemy settings")]
    [SerializeField] protected int maxhealth = 10;
    [SerializeField] protected GameObject target;
    [SerializeField] protected float speed;
    [SerializeField] protected float attackCooldown = 2f; 
    protected float lastAttackTime;
    [SerializeField] protected float idleTime = 2f;
    [SerializeField] protected float chaseRange = 5f;
    [SerializeField] protected Animator animator;
    [Header("VFX")]
    [SerializeField] protected ParticleSystem enemyDamageParticles;
    
    protected NavMeshAgent agent;
    protected EnemyState currentState;

    protected float distance;
    protected int health;
    protected float idleTimer;
    protected float stunTimer;
    protected float stunDuration;

    protected virtual void Awake()
    {
        gameObject.tag = "Enemy";
        waveSpawner = FindFirstObjectByType<WaveSpawner>();
        agent = GetComponent<NavMeshAgent>();
        health = maxhealth;
        animator = GetComponent<Animator>();
    }
    protected virtual void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        agent.speed = speed;
    }
    protected virtual void Update()
    {
        switch (currentState)
        {
            case EnemyState.Intro:
                break;

            case EnemyState.Idle:
                break;

            case EnemyState.Patrol:
                break;

            case EnemyState.Chase:
                HandleChase();
                break;

            case EnemyState.Stunned:
                break;

            case EnemyState.Attack:
                break;

            case EnemyState.Dead:
                break;
        }
    }
    //protected virtual void HandleIdle()
    //{
    //    idleTimer += Time.deltaTime;
    //    if (idleTimer >= idleTime)
    //    {
    //        idleTimer = 0f;
    //        SwitchState(EnemyState.Patrol);
    //    }
    //}
    protected virtual void HandleChase()
    {
        if (agent == null || target == null) return;

        agent.SetDestination(target.transform.position);

        float dist = Vector2.Distance(transform.position, target.transform.position);
        if (dist > chaseRange)
        {
            SwitchState(EnemyState.Idle);
        }
    }
    //protected virtual void HandleStunned()
    //{
    //    Debug.Log("Handling Stunned: Timer = " + stunTimer + " / " + stunDuration);
    //    stunTimer += Time.deltaTime;
    //    if (stunTimer >= stunDuration)
    //    {
    //        agent.isStopped = false;
    //        SwitchState(EnemyState.Patrol);
    //        animator.ResetTrigger("Stun");
    //        animator.SetBool("IsStuned",false);
    //    }
    //}
    public virtual void Stun(float duration)
    {
        if (agent != null)
        {
            agent.ResetPath();
            agent.isStopped = true;
        }

        animator.SetTrigger("Stun");
        animator.SetBool("IsStuned", true);

        SwitchState(EnemyState.Stunned);
        StartCoroutine(StunTimer(duration));
    }
    protected virtual IEnumerator StunTimer(float duration)
    {
        yield return new WaitForSeconds(duration);

        if (agent != null)
            agent.isStopped = false;

        animator.SetBool("IsStuned", false);
        animator.ResetTrigger("Stun");

        SwitchState(EnemyState.Patrol); 
    }
    /// <summary>
    /// Damages this enemy by a given amount
    /// </summary>
    /// <param name="damage">The amount of damage to apply</param>
    public virtual void DamageEnemy(int damage)
    {
        health -= damage;
        Debug.Log("Enemy damaged. New health is " + health);
        Instantiate(enemyDamageParticles, transform.position, Quaternion.identity);
        if (health <= 0) KillEnemy();
    }
    /// <summary>
    /// Causes the current enemy to die
    /// </summary>
    protected virtual void KillEnemy()
    {
        Debug.Log("Enemy killed!");
        currentState = EnemyState.Dead;
        if (agent != null)
        {
            agent.ResetPath();
            agent.isStopped = true;
        }
        animator.SetTrigger("Dead"); 
        //waveSpawner.DecrementEnemyCount();
        Destroy(gameObject, 1.5f); 
    }
    public void OnIntroComplete()
    {
        animator.SetTrigger("IntroDone");
    }
    protected virtual void SwitchState(EnemyState newState)
    {
        if (currentState == newState) return;
        currentState = newState;

        switch (newState)
        {
            case EnemyState.Idle:
                idleTimer = 0f;
                animator.SetFloat("Speed", 0f);
                break;

            case EnemyState.Patrol:
                animator.SetFloat("Speed", 1f);
                break;

            case EnemyState.Chase:
                animator.SetBool("HasTarget", true);
                break;

            case EnemyState.Stunned:
                break;

            case EnemyState.Attack:
                animator.SetTrigger("Attack");
                break;

            case EnemyState.Dead:
                animator.SetTrigger("Dead");
                break;
        }
    }
}

