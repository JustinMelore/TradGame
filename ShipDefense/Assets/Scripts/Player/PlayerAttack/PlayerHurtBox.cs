using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Hurtbox used by player to damage enemies
/// </summary>
public class PlayerHurtbox : Hurtbox
{
    private HashSet<Enemy> damagedEnemies = new HashSet<Enemy>();
    
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponentInParent<Enemy>();
            Debug.Log(enemy);
            if (enemy != null && !damagedEnemies.Contains(enemy))
            {
                enemy.DamageEnemy(damage);
                damagedEnemies.Add(enemy);
                Debug.Log("Player hit enemy!");
            }
        }
    }

    protected void OnEnable()
    {
        damagedEnemies.Clear();
    }
}