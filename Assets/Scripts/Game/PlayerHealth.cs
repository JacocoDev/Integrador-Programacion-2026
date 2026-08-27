using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;

    [Header("Health")]
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth = 3;

    private void Awake()
    {
        maxHealth = GameSettings.playerHealth;

        if (maxHealth < 1)
            maxHealth = 1;

        currentHealth = maxHealth;
    }

    public void TakeDamage()
    {
        if (!gameManager.IsGameActive())
            return;

        currentHealth--;

        if (currentHealth < 0)
            currentHealth = 0;

        if (currentHealth <= 0)
            gameManager.GameOver();
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}