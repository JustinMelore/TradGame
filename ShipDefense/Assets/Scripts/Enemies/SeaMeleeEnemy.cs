using UnityEngine;

/// <summary>
/// Handles behavior for melee enemies that spawn in the sea and attck the ship.
/// </summary>
public class SeaMeleeEnemy : MeleeEnemy
{
    [Header("Sea Melee Enemy Settings")]
    [SerializeField] private GameObject enemyHitbox;


    //TODO Implement wave spawner implementation
    protected override void Awake()
    {
        target = FindFirstObjectByType<PlayerController>().gameObject;
        waveSpawner = FindFirstObjectByType<WaveSpawner>();
        Vector2 direction = target.transform.position - transform.position;
        //In order for this to work, the SeaMeleeEnemy must be inside a container object
        if (direction.x < 0)
        {
            transform.parent.localScale = new Vector3(-transform.parent.localScale.x, transform.parent.localScale.y, transform.parent.localScale.z);
        }
        lastAttackTime = Time.time;
        health = maxhealth;
    }

    protected override void Start()
    {

    }

    protected override void Update()
    {
        if (Time.time - lastAttackTime >= attackCooldown && currentState != EnemyState.Stunned && currentState != EnemyState.Dead)
        {
            HandleAttack();
            lastAttackTime = Time.time;
        }
    }

    public override void OnAttackEnd()
    {
        isAttacking = false;
        animator.SetBool("Attacking", false);
        canParry = false;
        hurtbox.Deactivate();
    }
    
    public void MakeVulnerable()
    {
        enemyHitbox.SetActive(true);
    }

    public void MakeInvulnerable()
    {
        enemyHitbox.SetActive(false);
    }

    private void OnDestroy()
    {
        Destroy(transform.parent.gameObject);
    }
}
