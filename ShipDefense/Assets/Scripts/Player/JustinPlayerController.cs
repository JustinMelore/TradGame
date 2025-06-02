using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles actions taken by the player
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Rigidbody2D player;
    [SerializeField] private Transform attackDirection;
    [SerializeField] private Collider2D playerHitbox;
    [SerializeField] private Collider2D playerHurtBox;
    [SerializeField] private Collider2D playerParryBox;
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private AudioSource audioSource;

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

    [Header("Sounds")]
    [SerializeField] private AudioClip swordSwing;

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

    /// <summary>
    /// Triggers when the player presses one of the movement keys
    /// </summary>
    /// <param name="input"></param>
    private void OnMove(InputValue input)
    {
        movementDirection = input.Get<Vector2>().normalized;        
    }

    /// <summary>
    /// Triggers when the player presses the dodge button
    /// </summary>
    private void OnDodge()
    {
        if (isDodging) return;
        isDodging = true;
        currentDodgeDistance = 0f;
        preDodgePosition = player.position;
        dodgeDirection = (movementDirection == Vector2.zero) ? new Vector2(1, 0) : movementDirection;
        playerHitbox.enabled = false;
    }

    /// <summary>
    /// Triggers when the player moves their mouse
    /// </summary>
    /// <param name="input"></param>
    private void OnMousePos(InputValue input)
    {
        Vector2 mouseInput = input.Get<Vector2>();
        mousePosition = playerCamera.ScreenToWorldPoint(new Vector3(mouseInput.x, mouseInput.y, 1f));
    }

    /// <summary>
    /// Triggers when the player presses the parry button
    /// </summary>
    private void OnParry()
    {
        if (isParrying || parryOnCooldown || isDodging || isAttacking) return;
        currentParryTime = 0f;
        isParrying = true;
        playerParryBox.enabled = true;
        //Debug.Log("Parrying!");
        //TODO delete this in the future
        playerParryBox.transform.GetComponent<SpriteRenderer>().enabled = true;
    }

    /// <summary>
    /// Triggers when the player presses the attack button
    /// </summary>
    private void OnAttack()
    {
        if (isAttacking || attackOnCooldown || isDodging || isParrying) return;
        currentAttackTime = 0f;
        isAttacking = true;
        playerHurtBox.enabled = true;
        //Debug.Log("Attacking!");
        //TODO delete this in the future
        playerHurtBox.transform.GetComponent<SpriteRenderer>().enabled = true;
        PlaySound(swordSwing);
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

    /// <summary>
    /// Changes the player's current attack direction to match up with their mouse position
    /// </summary>
    private void UpdateAttackDirection()
    {
        Vector3 positionDifference = (mousePosition - attackDirection.position).normalized;
        float rotationAmount = Mathf.Atan2(positionDifference.y, positionDifference.x) * Mathf.Rad2Deg;
        attackDirection.localRotation = Quaternion.Euler(new Vector3(0f, 0f, rotationAmount + 90f));
    }

    /// <summary>
    /// Returns the player's current attack direction
    /// </summary>
    /// <returns></returns>
    public Vector3 GetAttackDirection()
    {
        return -attackDirection.up;
    }

    /// <summary>
    /// Determines when the dodge ends
    /// </summary>
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

    /// <summary>
    /// Determines when the parry ends and goes on cooldown
    /// </summary>
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

    /// <summary>
    /// Determines when the parry's cooldown has ended
    /// </summary>
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

    /// <summary>
    /// Determines when the attack should end and go on cooldown
    /// </summary>
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

    /// <summary>
    /// Determines when the attack cooldown has ended
    /// </summary>
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

    /// <summary>
    /// Returns the player's attack damage
    /// </summary>
    /// <returns></returns>
    public int GetAttackDamage()
    {
        return damage;
    }

    /// <summary>
    /// Damages the player by a given amount
    /// </summary>
    /// <param name="damage">The amount of damage to apply to the player</param>
    public void DamagePlayer(int damage)
    {
        health -= damage;
        Debug.Log("Player damage; new health: "+health);
        if (health <= 0) KillPlayer();
    }

    /// <summary>
    /// Causes the player to die
    /// </summary>
    private void KillPlayer()
    {
        Debug.Log("Player died!");
        Destroy(gameObject);
        waveManager.FailWave();
    }

    /// <summary>
    /// Helper method for playing a given audio clip. Will likely be moved to separate utility class in the future.
    /// </summary>
    /// <param name="audio"></param>
    private void PlaySound(AudioClip audio)
    {
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.PlayOneShot(audio);
    }
}
