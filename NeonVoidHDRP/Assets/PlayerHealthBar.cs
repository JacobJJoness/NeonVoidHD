using UnityEngine;
using UnityEngine.UI;
using System.Collections; // This namespace includes IEnumerator

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;  // Reference to the HealthBar script managing the UI slider
    public GameObject deathMessage;  // Assign this in the Unity Editor, assumed to be initially inactive
    public float regenRate = 1;  // Health points regenerated per second

    private Vector3 initialPosition;  // Cache initial position of the player
    private bool isDead = false;  // Track if player is dead
    private float lastDamageTime;  // Track the last time the player took damage

    void Start()
    {
        initialPosition = transform.position;  // Store the initial position at start
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);  // Set the maximum health on the health bar
            healthBar.SetHealth(currentHealth);  // Set the current health on the health bar
        }
        lastDamageTime = Time.time;  // Initialize the last damage time
        StartCoroutine(RegenerateHealth());  // Start the regeneration coroutine
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;  // Ignore damage if player is already dead

        currentHealth -= damage;
        lastDamageTime = Time.time;  // Update last damage time

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);  // Update the health bar when taking damage
        }

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    private IEnumerator RegenerateHealth()
    {
        while (true)
        {
            yield return new WaitForSeconds(10);  // Wait for 10 seconds before starting regeneration
            if (Time.time >= lastDamageTime + 10 && currentHealth < maxHealth && !isDead)
            {
                while (currentHealth < maxHealth && Time.time >= lastDamageTime + 10)
                {
                    currentHealth += Mathf.FloorToInt(regenRate);  // Increase health by regenRate per second
                    if (healthBar != null)
                    {
                        healthBar.SetHealth(currentHealth);  // Update health bar
                    }
                    yield return new WaitForSeconds(1);  // Wait for 1 second before adding more health
                }
            }
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Player is Dead!");
        ShowDeathMessage(true);
        gameObject.SetActive(false);
        Invoke("RespawnPlayer", 3f);
    }

    private void ShowDeathMessage(bool show)
    {
        if (deathMessage != null)
        {
            deathMessage.SetActive(show);
        }
    }

    private void RespawnPlayer()
    {
        gameObject.SetActive(true);
        transform.position = initialPosition;
        currentHealth = maxHealth;
        lastDamageTime = Time.time;  // Reset damage timer
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
        isDead = false;
        ShowDeathMessage(false);
        Debug.Log("Player Respawned");
    }
}
