using UnityEngine;

/// <summary>
/// Script to define enemy behavior. This script is mostly for testing purposes and will later be exchanged with more specific scripts for different enemy types
/// </summary>
public class Enemy : MonoBehaviour
{
    [SerializeField] private Rigidbody2D enemy;
    [SerializeField] private WaveManager waveManager;

    [Header("Enemy settings")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private int health;
    [SerializeField] private float fireRate;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveDistance;

    private Vector2 startPos;
    [SerializeField] private bool movingUp;

    [Header("VFX")]
    [SerializeField] private ParticleSystem enemyDamageParticles;

    private float currentFireTime;

    private void Start()
    {
        startPos = transform.position;
    }
    private void Update()
    {
        currentFireTime += Time.deltaTime;
        if (currentFireTime >= fireRate) FireProjectile();

        Vector2 newPos = enemy.position;
        if (movingUp)
        {
            newPos.y += moveSpeed * Time.deltaTime;
            if (newPos.y >= startPos.y + moveDistance)
            {
                newPos.y = startPos.y + moveDistance;
                movingUp = false;
            }
        }
        else
        {
            newPos.y -= moveSpeed * Time.deltaTime;
            if (newPos.y <= startPos.y - moveDistance)
            {
                newPos.y = startPos.y - moveDistance;
                movingUp = true;
            }
        }

        enemy.MovePosition(newPos);
    }

    /// <summary>
    /// Fires a projectile in the enemy's current direction
    /// </summary>
    private void FireProjectile()
    {
        currentFireTime = 0f;
        Vector3 projectileSpawnPosition = transform.position + transform.right * 2;
        Projectile firedProjectile = Instantiate(projectile, new Vector2(projectileSpawnPosition.x, projectileSpawnPosition.y), transform.rotation).GetComponent<Projectile>();
        //firedProjectile.Owner = gameObject;
        firedProjectile.ChangeMoveDirection(transform.right);
    }

    /// <summary>
    /// Damages this enemy by a given amount
    /// </summary>
    /// <param name="damage">The amount of damage to apply</param>
    public void DamageEnemy(int damage)
    {
        health -= damage;
        Debug.Log("Enemy damaged. New health is " + health);
        Instantiate(enemyDamageParticles, transform.position, Quaternion.identity);
        if (health <= 0) KillEnemy();
    }

    /// <summary>
    /// Causes the current enemy to die
    /// </summary>
    private void KillEnemy()
    {
        Debug.Log("Enemy killed!");
        waveManager.DecrementEnemyCount();
        Destroy(gameObject);
    }
}
