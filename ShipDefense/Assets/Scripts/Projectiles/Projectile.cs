using System;
using UnityEngine;

/// <summary>
/// Script for handling basic projectile behavior
/// </summary>
public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody2D projectile;
    [SerializeField] private Collider2D enemyCollider;
    [SerializeField] private Collider2D playerCollider;

    [Header("Projectile Settings")]
    [SerializeField] private int damage;
    [SerializeField] private float speed;
    [SerializeField] private float projectileLifetime; 


    private PlayerController player;
    private float currentLifetime;

    //public GameObject Owner { get; set; }

    private void Awake()
    {
        player = FindFirstObjectByType<PlayerController>();
        enemyCollider.enabled = true;
        playerCollider.enabled = false;
        currentLifetime = 0f;
    }

    private void Update()
    {
        currentLifetime += Time.deltaTime;
        if (currentLifetime >= projectileLifetime) Destroy(gameObject);
    }

    /// <summary>
    /// Redirects the projectile to move in a new given direction
    /// </summary>
    /// <param name="newVelocity">The new direction of the projectile</param>
    public void ChangeMoveDirection(Vector2 newVelocity)
    {
        projectile.linearVelocity = newVelocity * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {        
        GameObject collided = collision.collider.gameObject;
        //Debug.Log(LayerMask.LayerToName(collided.layer));
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerParryTag"))
        {
            Debug.Log("Player deflected projectile!");
            enemyCollider.enabled = false;
            playerCollider.enabled = true;
            ChangeMoveDirection(player.GetAttackDirection());
            currentLifetime = 0f;
            collision.transform.parent.GetComponentInParent<Animator>().SetTrigger("Deflected");
            collision.transform.parent.GetComponentInParent<PlayerController>().SpawnDeflectParticles();
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    internal void Reflect(Vector2 newDirection)
    {
        Debug.Log("Projectile reflected!");
        enemyCollider.enabled = false;
        playerCollider.enabled = true;
        ChangeMoveDirection(newDirection);
        currentLifetime = 0f;
    }
}

