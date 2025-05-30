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
        GameObject collided = collision.collider.gameObject;
        if (collided.layer == LayerMask.NameToLayer("Player") && Owner != collided.transform.parent.gameObject) collided.GetComponentInParent<PlayerController>().DamagePlayer(damage);
        Destroy(gameObject);
    }
}
