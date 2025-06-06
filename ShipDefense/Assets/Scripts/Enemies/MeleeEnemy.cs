using UnityEngine;

public class MeleeEnemy : Enemy
{
    [Header("Melee Settings")]
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private int meleeDamage = 5;
    [SerializeField] private EnemyHurtbox hurtbox;
    [SerializeField] private Transform attackDirection;
    [SerializeField] private float attackDuration;

    protected override void Update()
    {
        base.Update();

        if (target != null)
        {
            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
            float attackOffset = 0.6f; 
            attackDirection.position = transform.position + directionToTarget * attackOffset;

            float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
            attackDirection.rotation = Quaternion.Euler(0f, 0f, angle);

            if (Vector2.Distance(transform.position, target.transform.position) <= attackRange)
            {
                if (Time.time - lastAttackTime >= attackCooldown)
                {
                    Attack();
                    lastAttackTime = Time.time;
                }
            }
        }
    }

    private void Attack()
    {
        hurtbox.transform.position = attackDirection.position;
        hurtbox.transform.rotation = attackDirection.rotation;
        hurtbox.Activate(meleeDamage);
        StartCoroutine(DeactivateHurtboxAfterDelay(attackDuration));
    }

    private System.Collections.IEnumerator DeactivateHurtboxAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        hurtbox.Deactivate();
    }
}
