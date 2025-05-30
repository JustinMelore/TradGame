using UnityEngine;

/// <summary>
/// Script to define enemy behavior. This script is mostly for testing purposes and will later be exchanged with more specific scripts for different enemy types
/// </summary>
public class Enemy : MonoBehaviour
{
    [SerializeField] private Rigidbody2D enemy;

    [Header("Enemy settings")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private int health;
    [SerializeField] private float fireRate;

    private float currentFireTime;

    private void Update()
    {
        currentFireTime += Time.deltaTime;
        if (currentFireTime >= fireRate) FireProjectile();
    }

    private void FireProjectile()
    {
        Debug.Log("Firing projectile");
        currentFireTime = 0f;
        Vector3 projectileSpawnPosition = transform.position + transform.right * 2;
        Projectile firedProjectile = Instantiate(projectile, new Vector2(projectileSpawnPosition.x, projectileSpawnPosition.y), transform.rotation).GetComponent<Projectile>();
        firedProjectile.Owner = gameObject;
        firedProjectile.ChangeMoveDirection(transform.right);
    }

}
