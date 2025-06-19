using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Script to define enemy behavior. This script is mostly for testing purposes and will later be exchanged with more specific scripts for different enemy types
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
    }
    protected virtual void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        agent.speed = speed;
        currentState = EnemyState.Idle;

    }
    protected virtual void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                HandleIdle();
                break;

            case EnemyState.Chase:
                HandleChase();
                break;

            case EnemyState.Stunned:
                HandleStunned();
                break;

            case EnemyState.Dead:
                break;
        }
    }
    protected virtual void HandleIdle()
    {
        idleTimer += Time.deltaTime;
        if (idleTimer >= idleTime)
        {
            idleTimer = 0f;
        }
    }
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
    protected virtual void HandleStunned()
    {
        stunTimer += Time.deltaTime;
        if (stunTimer >= stunDuration)
        {
            SwitchState(EnemyState.Idle);
        }
    }
    public virtual void Stun(float duration)
    {
        stunTimer = 0f;
        stunDuration = duration;
        SwitchState(EnemyState.Stunned);
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
        agent.ResetPath();
        waveSpawner.DecrementEnemyCount();
        Destroy(gameObject);
    }
    protected virtual void SwitchState(EnemyState newState)
    {
        currentState = newState;

        if (agent != null && currentState == EnemyState.Stunned)
        {
            agent.ResetPath(); // Stop movement while stunned
        }
    }
}

