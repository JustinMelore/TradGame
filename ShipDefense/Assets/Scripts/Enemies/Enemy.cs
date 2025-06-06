using UnityEngine;
/// <summary>
/// Script to define enemy behavior. This script is mostly for testing purposes and will later be exchanged with more specific scripts for different enemy types
/// </summary>
public class Enemy : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D enemy;
    protected WaveSpawner waveSpawner;

    [Header("Common Enemy settings")]
    [SerializeField] protected int health;
    [SerializeField] protected GameObject target;
    [SerializeField] protected float speed;
    [SerializeField] protected float attackCooldown = 2f; 
    protected float lastAttackTime;

    [Header("VFX")]
    [SerializeField] protected ParticleSystem enemyDamageParticles;

    protected float distance;
    protected virtual void Awake()
    {
        gameObject.tag = "Enemy";
        waveSpawner = FindFirstObjectByType<WaveSpawner>();
    }
    protected virtual void Start()
    {
        
    }
    protected virtual void Update()
    {
        Chaser();
    }

    protected virtual void Chaser()
    {
        if (target)
        {
            distance = Vector2.Distance(transform.position, target.transform.position);
            Vector2 direction = target.transform.position - transform.position;
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        }
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
        waveSpawner.DecrementEnemyCount();
        Destroy(gameObject);
    }
}
