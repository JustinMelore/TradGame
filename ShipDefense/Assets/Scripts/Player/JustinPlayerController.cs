using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Rigidbody2D player;
    [SerializeField] private Transform attackDirection;
    [SerializeField] private Collider2D playerHitbox;
    [SerializeField] private Collider2D playerHurtBox;
    [SerializeField] private Collider2D playerParryBox;
    [SerializeField] private WaveManager waveManager;

    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float dodgeSpeed;
    [SerializeField] private float dodgeDistance;

    [Header("Player Stats Settings")]
    [SerializeField] private int health;

    [Header("Combat Settings")]
    [SerializeField] private float parryDuration;
    [SerializeField] private float parryCooldown;
    [SerializeField] private int damage;
    [SerializeField] private float attackDuration;
    [SerializeField] private float attackCooldown;

    private Vector3 mousePosition;
    private Vector2 movementDirection;
    private bool isDodging;
    private float currentDodgeDistance;
    private Vector2 preDodgePosition;
    private Vector2 dodgeDirection;

    private bool isParrying;
    private bool parryOnCooldown;
    private float currentParryTime;
    private float currentParryCooldown;

    private bool isAttacking;
    private bool attackOnCooldown;
    private float currentAttackTime;
    private float currentAttackCooldown;

    private void Awake()
    {
        playerHurtBox.enabled = false;
        playerParryBox.enabled = false;
    }

    private void OnMove(InputValue input)
    {
        movementDirection = input.Get<Vector2>().normalized;        
    }

    private void OnDodge()
    {
        if (isDodging) return;
        isDodging = true;
        currentDodgeDistance = 0f;
        preDodgePosition = player.position;
        dodgeDirection = (movementDirection == Vector2.zero) ? new Vector2(1, 0) : movementDirection;
        playerHitbox.enabled = false;
    }

    private void OnMousePos(InputValue input)
    {
        Vector2 mouseInput = input.Get<Vector2>();
        mousePosition = playerCamera.ScreenToWorldPoint(new Vector3(mouseInput.x, mouseInput.y, 1f));
    }

    private void OnParry()
    {
        if (isParrying || parryOnCooldown) return;
        currentParryTime = 0f;
        isParrying = true;
        playerParryBox.enabled = true;
        //Debug.Log("Parrying!");
        //TODO delete this in the future
        playerParryBox.transform.GetComponent<SpriteRenderer>().enabled = true;
    }

    private void OnAttack()
    {
        if (isAttacking || attackOnCooldown) return;
        currentAttackTime = 0f;
        isAttacking = true;
        playerHurtBox.enabled = true;
        //Debug.Log("Attacking!");
        //TODO delete this in the future
        playerHurtBox.transform.GetComponent<SpriteRenderer>().enabled = true;
    }

    void FixedUpdate()
    {
        UpdateAttackDirection();
        if (!isDodging)
        {
            player.linearVelocity = movementDirection * movementSpeed;
        } else
        {
            PerformDodge();
        }
        if(isParrying)
        {
            PerformParry();
        } else if(parryOnCooldown)
        {
            RecoverParryCooldown();
        }
        if (isAttacking)
        {
            PerformAttack();
        }
        else if (attackOnCooldown)
        {
            RecoverAttackCooldown();
        }
    }

    private void UpdateAttackDirection()
    {
        Vector3 positionDifference = (mousePosition - attackDirection.position).normalized;
        float rotationAmount = Mathf.Atan2(positionDifference.y, positionDifference.x) * Mathf.Rad2Deg;
        attackDirection.localRotation = Quaternion.Euler(new Vector3(0f, 0f, rotationAmount + 90f));
    }

    public Vector3 GetAttackDirection()
    {
        return -attackDirection.up;
    }

    private void PerformDodge()
    {
        if(currentDodgeDistance < dodgeDistance)
        {
            currentDodgeDistance = Vector2.Distance(player.position, preDodgePosition);
            player.linearVelocity = dodgeDirection * dodgeSpeed;
        } else
        {
            isDodging = false;
            playerHitbox.enabled = true;
        }
    }

    private void PerformParry()
    {
        if(currentParryTime < parryDuration)
        {
            currentParryTime += Time.deltaTime;
        } else
        {
            isParrying = false;
            playerParryBox.enabled = false;
            currentParryCooldown = 0f;
            parryOnCooldown = true;
            //Debug.Log("Parry ended. Starting cooldown");
            //TODO delete this in the future
            playerParryBox.transform.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private void RecoverParryCooldown()
    {
        if(currentParryCooldown < parryCooldown)
        {
            currentParryCooldown += Time.deltaTime;
        } else
        {
            parryOnCooldown = false;
            //Debug.Log("Parry cooldown ended");
        }
    }

    private void PerformAttack()
    {
        if (currentAttackTime < attackDuration)
        {
            currentAttackTime += Time.deltaTime;
        }
        else
        {
            isAttacking = false;
            playerHurtBox.enabled = false;
            currentAttackCooldown = 0f;
            attackOnCooldown = true;
            //Debug.Log("Attack ended. Starting cooldown");
            //TODO delete this in the future
            playerHurtBox.transform.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private void RecoverAttackCooldown()
    {
        if (currentAttackCooldown < attackCooldown)
        {
            currentAttackCooldown += Time.deltaTime;
        }
        else
        {
            attackOnCooldown = false;
            //Debug.Log("Attack cooldown ended");
        }
    }

    public int GetAttackDamage()
    {
        return damage;
    }

    public void DamagePlayer(int damage)
    {
        health -= damage;
        Debug.Log("Player damage; new health: "+health);
        if (health <= 0) KillPlayer();
    }

    private void KillPlayer()
    {
        Debug.Log("Player died!");
        Destroy(gameObject);
        waveManager.FailWave();
    }
}
