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
        Debug.Log("New projectile velocity: " + projectile.linearVelocity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision);
        Destroy(gameObject);
    }
}
