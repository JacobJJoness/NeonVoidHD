using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
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
        }
    }
}
