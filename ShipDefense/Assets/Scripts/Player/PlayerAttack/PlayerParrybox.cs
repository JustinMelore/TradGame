using UnityEngine;

public class PlayerParrybox : Hurtbox
{
    [SerializeField] private float stunDuration = 2.0f;
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        Projectile projectile = collision.GetComponent<Projectile>();
        if (projectile != null)
        {
            Debug.Log("Parried a projectile!");
            projectile.Reflect(GetComponentInParent<PlayerController>().GetAttackDirection());
        }

        MeleeEnemy meleeEnemy = collision.GetComponentInParent<MeleeEnemy>();
        if (meleeEnemy != null)
        {
            Debug.Log("Parried a melee enemy!");
            meleeEnemy.Stun(this.GetStunDuration());
        }
    }

    public float GetStunDuration()
    {
        return stunDuration;
    }
}