using UnityEngine;

public class MeleeEnemy : Enemy
{
    [Header("Melee Settings")]
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private int meleeDamage = 1;

    private float lastAttackTime;

    protected override void Update()
    {
        base.Update();

        if (target != null && Vector2.Distance(transform.position, target.transform.position) <= attackRange)
        {
            if (Time.time - lastAttackTime > attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
    }

    private void Attack()
    {
        Debug.Log("Melee enemy attacks!");
        target.GetComponent<PlayerController>()?.DamagePlayer(meleeDamage);
    }
}
