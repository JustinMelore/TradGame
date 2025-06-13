using UnityEngine;

/// <summary>
/// Handles the behavior of the ship, specifically in regards to its health
/// </summary>
public class Ship : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerHealthUI healthBar;

    [Header("Ship Settings")]
    [SerializeField] private int shipHealth;
    private int currentShipHealth;

    private void Awake()
    {
        currentShipHealth = shipHealth;
    }

    /// <summary>
    /// Damages the ship's health by a given amount
    /// </summary>
    /// <param name="damage"></param>
    public void DamageShip(int damage)
    {
        currentShipHealth -= damage;
        healthBar.SetHealth(currentShipHealth, shipHealth);
        if (currentShipHealth <= 0) DestroyShip();
    }

    /// <summary>
    /// Destroys the ship, causing a game over
    /// </summary>
    private void DestroyShip()
    {
        gameManager.FailGame();
    }
}
