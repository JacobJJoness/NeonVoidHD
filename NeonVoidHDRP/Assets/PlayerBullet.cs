using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public int damageAmount = 20; // Damage the bullet does

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
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

