using UnityEngine;

public class NPC : MonoBehaviour
{
    public DialogueManager dialogueManager;
    private bool playerIsNear = false; // Flag to check if the player is near
    public string missionName = "Kill the Crawler!!"; // Assign a mission name specific to this NPC
    public string missionDialogue = "Hi Croc Men, I have a mission for you: Kill the Crawler!! Would you like to do it? Yes or No?";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure the player GameObject has the tag "Player"
        {
            playerIsNear = true; // Set flag to true when player enters the trigger
            Debug.Log("Player is near the NPC. Press 'E' to interact.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false; // Reset flag when player exits the trigger
            dialogueManager.dialoguePanel.SetActive(false); // Hide dialogue when player walks away
            Debug.Log("Player has left the NPC.");
        }
    }

    private void Update()
    {
        // Check if player is near and player presses 'E'
        if (playerIsNear && Input.GetKeyDown(KeyCode.E))
        {
            dialogueManager.ShowDialogue(missionDialogue, missionName);
        }
    }
}
