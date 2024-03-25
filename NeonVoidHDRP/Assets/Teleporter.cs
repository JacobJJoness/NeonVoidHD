using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            // Print a message to the Unity Console
            Debug.Log("Player nearby - teleporting to Main World");

            // Use SceneManager to load the "Main World" scene
            SceneManager.LoadScene("MainWorld");
        }
    }
}
