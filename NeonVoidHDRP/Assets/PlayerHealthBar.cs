using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;  // Reference to the HealthBar script managing the UI slider
    public GameObject deathMessage;  // Assign this in the Unity Editor, assumed to be initially inactive

    private Vector3 initialPosition;  // Cache initial position of the player
    private bool isDead = false;  // Track if player is dead

    void Start()
    {
        initialPosition = transform.position;  // Store the initial position at start
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);  // Set the maximum health on the health bar
            healthBar.SetHealth(currentHealth);  // Set the current health on the health bar
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;  // Ignore damage if player is already dead

        currentHealth -= damage;
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);  // Update the health bar when taking damage
        }

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Player is Dead!");  // Handle death (e.g., show death screen, stop player movement)
        ShowDeathMessage(true);
        gameObject.SetActive(false);  // Disable the player object instead of destroying it
        Invoke("RespawnPlayer", 3f);  // Schedule the respawn
    }

    private void ShowDeathMessage(bool show)
    {
        if (deathMessage != null)
        {
            deathMessage.SetActive(show);  // Toggle visibility of the death message
        }
    }

    private void RespawnPlayer()
    {
        gameObject.SetActive(true);  // Re-enable the player object
        transform.position = initialPosition;  // Reset position to initial
        currentHealth = maxHealth;  // Reset health to maximum
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);  // Update the health bar to reflect full health
        }
        isDead = false;
        ShowDeathMessage(false);  // Hide the death message
        Debug.Log("Player Respawned");
    }
}
