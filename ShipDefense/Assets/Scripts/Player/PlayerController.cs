using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D player;

    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed;

    private Vector2 movementDirection;

    private void OnMove(InputValue input)
    {
        movementDirection = input.Get<Vector2>().normalized;
    }

    void FixedUpdate()
    {
        player.linearVelocity = movementDirection * movementSpeed;
    }
}
