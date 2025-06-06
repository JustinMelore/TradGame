using UnityEngine;

/// <summary>
/// Hurtbox used by player to damage enemies
/// </summary>
public class PlayerHurtbox : Hurtbox
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.DamageEnemy(damage);
                Debug.Log("Player hit enemy!");
            }
        }
    }
}