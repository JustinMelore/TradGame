using UnityEngine;

/// <summary>
/// Handles behavior for melee enemies that spawn in the sea and attck the ship.
/// </summary>
public class SeaMeleeEnemy : MeleeEnemy
{

    protected override void Awake()
    {
        target = FindFirstObjectByType<PlayerController>().gameObject;
        Vector2 direction = target.transform.position - transform.position;
        if (direction.x < 0) transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        lastAttackTime = Time.time;
    }

    //protected override void Update()
    //{
    //    if (Time.time - lastAttackTime >= attackCooldown)
    //    {
    //        Attack();
    //        lastAttackTime = Time.time;
    //    }
    //}

    protected void Attack()
    {
        hurtbox.Activate(meleeDamage);
        StartCoroutine(DeactivateHurtboxAfterDelay(attackDuration));
    }

    protected System.Collections.IEnumerator DeactivateHurtboxAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        hurtbox.Deactivate();
    }
}
