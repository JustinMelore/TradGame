using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody2D projectile;
    [SerializeField] private Collider2D enemyCollider;
    [SerializeField] private Collider2D playerCollider;

    [Header("Projectile Settings")]
    [SerializeField] private int damage;
    [SerializeField] private float speed;


    private PlayerController player;

    //public GameObject Owner { get; set; }

    private void Awake()
    {
        player = FindFirstObjectByType<PlayerController>();
        enemyCollider.enabled = true;
        playerCollider.enabled = false;
    }

    public void ChangeMoveDirection(Vector2 newVelocity)
    {
        projectile.linearVelocity = newVelocity * speed;
        Debug.Log("New projectile velocity: " + projectile.linearVelocity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {        
        GameObject collided = collision.collider.gameObject;
        if (collided.layer == player.gameObject.layer)
        {
            collided.GetComponentInParent<PlayerController>().DamagePlayer(damage);
        } else if (collided.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("Projectile hit enemy!");
            collided.GetComponent<Enemy>().DamageEnemy(damage);
        }
            Destroy(gameObject);
    }
}
