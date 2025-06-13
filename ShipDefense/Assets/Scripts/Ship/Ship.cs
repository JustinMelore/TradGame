using UnityEngine;

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

    public void DamageShip(int damage)
    {
        currentShipHealth -= damage;
        healthBar.SetHealth(currentShipHealth, shipHealth);
        if (currentShipHealth <= 0) DestroyShip();
    }

    private void DestroyShip()
    {
        gameManager.FailGame();
    }
}
