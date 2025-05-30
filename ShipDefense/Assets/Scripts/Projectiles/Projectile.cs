using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody2D projectile;

    [Header("Projectile Settings")]
    [SerializeField] private int damage;
    [SerializeField] private float speed;


    public GameObject Owner { get; set; }

    public void ChangeMoveDirection(Vector2 newVelocity)
    {
        projectile.linearVelocity = newVelocity * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision);
    }
}
