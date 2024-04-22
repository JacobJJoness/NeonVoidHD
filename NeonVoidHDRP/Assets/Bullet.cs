using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damageAmount = 20; // Damage the bullet does

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the bullet hits the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Access the player's health script and apply damage
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                Debug.Log("Damage applied to player: " + damageAmount);
            }
            else
            {
                Debug.LogError("PlayerHealth component not found on the player object.");
            }
            Destroy(gameObject); // Destroy the bullet after it hits the player
        }
        // Check if the bullet hits the enemy
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            // Access the enemy's health script and apply damage
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount);
                Debug.Log("Damage applied to enemy: " + damageAmount);
            }
            else
            {
                Debug.LogError("EnemyHealth component not found on the enemy object.");
            }
            Destroy(gameObject); // Destroy the bullet after it hits the enemy
        }
    }
}
