using UnityEngine;

/// <summary>
/// Handles behavior for melee attack hurtboxes
/// </summary>
public class Hurtbox : MonoBehaviour
{
    //TODO Implement code for enemy
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hurtbox hit something");
        if (gameObject.layer == LayerMask.NameToLayer("PlayerHurtbox"))
        {

            if (gameObject.layer == LayerMask.NameToLayer("PlayerHurtbox"))
            {
                Enemy enemy = collision.GetComponent<Enemy>();
                if (enemy != null)
                {
                    PlayerController player = transform.parent?.parent?.GetComponent<PlayerController>();
                    if (player != null)
                    {
                        enemy.DamageEnemy(player.GetAttackDamage());
                    }
                    else
                    {
                        Debug.LogWarning("PlayerController not found on Hurtbox parent.");
                    }
                }
                else
                {
                    Debug.Log("Hurtbox hit something that is not an enemy: " + collision.name);
                }
            }
        }
    }
}
