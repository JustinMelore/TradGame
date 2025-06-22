using UnityEngine;

/// <summary>
/// Melee hurtbox for melee-type enemies
/// </summary>
public class EnemyHurtbox : Hurtbox

{
    private Ship ship;

    private void Awake()
    {
        ship = FindFirstObjectByType<Ship>();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponentInParent<PlayerController>();
            if (player != null)
            {
                player.DamagePlayer(damage);
                Debug.Log("Enemy hit the player! with Damage" + damage);
                Deactivate();
            }
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            SeaMeleeEnemy seaMeleeEnemy = GetComponentInParent<SeaMeleeEnemy>();
            if (seaMeleeEnemy != null)
            {
                ship.DamageShip(damage);
                Debug.Log("Enemy hit the ship for " + damage + " damage");
                Deactivate();
            }
        }
    }
}