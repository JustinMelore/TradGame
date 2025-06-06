using UnityEngine;

public class PlayerParrybox : Hurtbox
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        Projectile projectile = collision.GetComponent<Projectile>();
        if (projectile != null)
        {
            Debug.Log("Parried a projectile!");
            projectile.Reflect(GetComponentInParent<PlayerController>().GetAttackDirection());
        }
    }
}