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

    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float dodgeSpeed;
    [SerializeField] private float dodgeDistance;

    [Header("Player Stats Settings")]
    [SerializeField] private int health;

    [Header("Combat Settings")]
    [SerializeField] private float parryDuration;
    [SerializeField] private float parryCooldown;

    private Vector3 mousePosition;
    private Vector2 movementDirection;
    private bool isDodging;
    private bool isParrying;
    private bool parryOnCooldown;
    private float currentDodgeDistance;
    private float currentParryTime;
    private float currentParryCooldown;
    private Vector2 dodgeDirection;
    private Vector2 preDodgePosition;


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
        Debug.Log("Parrying!");
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
            Debug.Log("Parry ended. Starting cooldown");
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
            Debug.Log("Parry cooldown ended");
        }
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
    }
}
