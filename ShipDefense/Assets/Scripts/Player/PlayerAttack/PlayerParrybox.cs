using UnityEngine;

public class PlayerParrybox : Hurtbox
{
    [SerializeField] private Animator playerAnimator;
    private PlayerController player;
    [SerializeField] private float stunDuration = 2.0f;

    private void Awake()
    {
        player = playerAnimator.transform.GetComponent<PlayerController>();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        //Projectile projectile = collision.GetComponent<Projectile>();
        //if (projectile != null)
        //{
        //    Debug.Log("Parried a projectile!");
        //    projectile.Reflect(GetComponentInParent<PlayerController>().GetAttackDirection());
        //    playerAnimator.SetTrigger("Deflected");
        //}
        player.HealFromParry();
        MeleeEnemy meleeEnemy = collision.GetComponentInParent<MeleeEnemy>();
        if (meleeEnemy != null)
        {
            if (meleeEnemy.canParry)
            {
                Debug.Log("Parried a melee enemy!");
                meleeEnemy.Stun(this.GetStunDuration());
                playerAnimator.SetTrigger("Deflected");
                player.SpawnDeflectParticles();
            }
        }
    }

    public float GetStunDuration()
    {
        return stunDuration;
    }
}