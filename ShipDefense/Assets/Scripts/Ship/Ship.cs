using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the behavior of the ship, specifically in regards to its health
/// </summary>
public class Ship : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerHealthUI healthBar;
    [SerializeField] private GameObject shipBroken1;
    [SerializeField] private GameObject shipBroken2;
    [SerializeField] private GameObject shipBroken3;

    [Header("Ship Settings")]
    [SerializeField] private int shipHealth;
    private int currentShipHealth;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        currentShipHealth = shipHealth;
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
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
        if (currentShipHealth <= 700)
        {
            shipBroken1.SetActive(true);
        }
        if (currentShipHealth <= 500)
        {
            shipBroken2.SetActive(true);
        }
        if (currentShipHealth <= 300)
        {
            shipBroken3.SetActive(true);
        }
    }

    /// <summary>
    /// Destroys the ship, causing a game over
    /// </summary>
    private void DestroyShip()
    {
        gameManager.FailGame();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (gameManager == null) gameManager = FindFirstObjectByType<GameManager>();
        if (healthBar == null) healthBar = FindFirstObjectByType<Canvas>().transform.Find("ShipHealthBarContainer").Find("ShipHealthBar").GetComponent<PlayerHealthUI>();
        healthBar.SetHealth(currentShipHealth, shipHealth);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
