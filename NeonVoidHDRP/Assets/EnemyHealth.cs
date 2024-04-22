using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;  // Reference to the HealthBar script managing the UI slider
    public GameObject deathEffect;  // Optional: A prefab for visual effect upon death

    private Vector3 initialPosition;  // To store the initial position for potential respawning
    private bool isDead = false;  // To track the death state

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
        if (isDead) return;  // Ignore damage if already dead

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
        Debug.Log(gameObject.name + " is Dead!");
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);  // Show death effect
        }
        gameObject.SetActive(false);  // Disable the enemy object instead of destroying it
        Invoke("Respawn", 5f);  // Respawn enemy after a delay
    }

    private void Respawn()
    {
        gameObject.SetActive(true);
        transform.position = initialPosition;  // Reset position to initial
        currentHealth = maxHealth;  // Reset health to maximum
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);  // Update the health bar to reflect full health
        }
        isDead = false;
        Debug.Log(gameObject.name + " Respawned");
    }
}
