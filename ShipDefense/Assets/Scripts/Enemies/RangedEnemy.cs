using UnityEngine;

public class RangedEnemy : Enemy
{
    [Header("Ranged Settings")]
    [SerializeField] private GameObject projectile;

    private float currentFireTime;

    protected override void Update()
    {
        base.Update();

        if (target != null && Time.time - lastAttackTime >= attackCooldown)
        {
            FireProjectile();
            lastAttackTime = Time.time;
        }
    }

    private void FireProjectile()
    {
        if (target == null) return;

        Vector3 direction = (target.transform.position - transform.position).normalized;
        Vector3 spawnOffset = direction * 0.5f;

        Vector3 projectileSpawnPosition = transform.position + spawnOffset;
        Projectile firedProjectile = Instantiate(projectile, projectileSpawnPosition, Quaternion.identity).GetComponent<Projectile>();
        firedProjectile.ChangeMoveDirection(direction);
    }
}
