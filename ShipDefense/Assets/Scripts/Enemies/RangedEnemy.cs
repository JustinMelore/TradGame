using UnityEngine;

public class RangedEnemy : Enemy
{
    [Header("Ranged Settings")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private float fireRate;

    private float currentFireTime;

    protected override void Update()
    {
        base.Update();
        currentFireTime += Time.deltaTime;
        if (currentFireTime >= fireRate)
        {
            FireProjectile();
            currentFireTime = 0f;
        }
    }

    private void FireProjectile()
    {
        Vector3 projectileSpawnPosition = transform.position + transform.right * 2;
        Projectile firedProjectile = Instantiate(projectile, projectileSpawnPosition, transform.rotation).GetComponent<Projectile>();
        firedProjectile.ChangeMoveDirection(transform.right);
    }
}
