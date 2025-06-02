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

            collision.gameObject.GetComponent<Enemy>().DamageEnemy(transform.parent.parent.GetComponent<PlayerController>().GetAttackDamage());
        }
    }
}
