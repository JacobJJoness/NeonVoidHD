using UnityEngine;
using UnityEngine.UI;

public class RobotHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    public Slider healthSlider; // Reference to the health slider UI element

    private Vector3 initialPosition; // Initial position of the robot

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(currentHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }

    private void Die()
    {
        // Destroy the robot
        Destroy(gameObject);

        // Respawn the robot after a delay (optional)
        RespawnRobot(3f);
    }

    private void RespawnRobot(float delay)
    {
        Invoke("SpawnRobot", delay);
    }

    private void SpawnRobot()
    {
        // Instantiate a new instance of the robot prefab at the initial position
        GameObject newRobot = Instantiate(gameObject, initialPosition, Quaternion.identity);

        // Reset the health of the new robot
        RobotHealth robotHealth = newRobot.GetComponent<RobotHealth>();
        if (robotHealth != null)
        {
            robotHealth.currentHealth = robotHealth.maxHealth;
        }
    }

 

}
