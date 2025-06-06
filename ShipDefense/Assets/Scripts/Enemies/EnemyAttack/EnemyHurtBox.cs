using UnityEngine;

/// <summary>
/// Melee hurtbox for melee-type enemies
/// </summary>
public class EnemyHurtbox : Hurtbox

{
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
    }
}