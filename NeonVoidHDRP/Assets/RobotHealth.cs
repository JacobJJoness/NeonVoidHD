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
        initialPosition = transform.position;  // Store the initial position at start
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(currentHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            
            Die();
        }
    }




    private void Die()
    {
        MissionManager.Instance.CompleteMission("Punch a Robot to Death");
        // Destroy the robot
        Destroy(gameObject);

        // Respawn the robot after a delay (optional)
        RespawnRobot(3f);
    }

    private void RespawnRobot(float delay)
    {
        Invoke("SpawnRobot", delay);
    }

    public GameObject robotPrefab; // Assign this in the Unity editor

    private void SpawnRobot()
    {
        GameObject newRobot = Instantiate(robotPrefab, initialPosition, Quaternion.identity);
        RobotHealth robotHealth = newRobot.GetComponent<RobotHealth>();
        if (robotHealth != null)
        {
            robotHealth.currentHealth = robotHealth.maxHealth;
        }
    }



}
