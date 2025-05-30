using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D player;

    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float dodgeSpeed;
    [SerializeField] private float dodgeDistance;

    private Vector2 movementDirection;
    private bool isDodging;
    private float currentDodgeDistance;
    private Vector2 dodgeDirection;
    private Vector2 preDodgePosition;


    private void OnMove(InputValue input)
    {
        movementDirection = input.Get<Vector2>().normalized;        
    }

    private void OnDodge()
    {
        if (isDodging) return;
        Debug.Log("Dodge pressed!");
        isDodging = true;
        currentDodgeDistance = 0f;
        preDodgePosition = player.position;
        dodgeDirection = (movementDirection == Vector2.zero) ? new Vector2(1, 0) : movementDirection;
    }

    void FixedUpdate()
    {
        if(!isDodging)
        {
            player.linearVelocity = movementDirection * movementSpeed;
        } else
        {
            PerformDodge();
        }
    }

    private void PerformDodge()
    {
        if(currentDodgeDistance < dodgeDistance)
        {
            currentDodgeDistance = Vector2.Distance(player.position, preDodgePosition);
            player.linearVelocity = dodgeDirection * dodgeSpeed;
        } else
        {
            Debug.Log("Dodge ended!");
            isDodging = false;
        }
    }
}
