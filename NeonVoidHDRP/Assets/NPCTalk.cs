using UnityEngine;

public class NPCTalk : MonoBehaviour
{
    public GameObject portalPrefab;
    private bool isPlayerInside = false;
    private bool hasInteracted = false;

    private void Update()
    {
        // Check if the player is inside the NPC's trigger zone
        if (isPlayerInside)
        {
            // If the player hasn't interacted yet and presses the "E" key, toggle the portal
            if (!hasInteracted && Input.GetKeyDown(KeyCode.E))
            {
                TogglePortal();
            }
            // If the portal is active and the player presses the "E" key again, deactivate the portal
            else if (hasInteracted && Input.GetKeyDown(KeyCode.E))
            {
                DeactivatePortal();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered NPC's trigger zone.");
            isPlayerInside = true; // Set flag to indicate player is inside the trigger zone
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited NPC's trigger zone.");
            isPlayerInside = false; // Reset flag when player exits the trigger zone
            // If the player hasn't interacted yet, deactivate the portal object prefab
            if (!hasInteracted)
            {
                portalPrefab.SetActive(false);
            }
        }
    }

    private void TogglePortal()
    {
        Debug.Log("Player interacted with NPC.");
        // Toggle the portal object prefab
        portalPrefab.SetActive(!portalPrefab.activeSelf);
        hasInteracted = true; // Set the flag to indicate interaction
    }

    private void DeactivatePortal()
    {
        Debug.Log("Player deactivated the portal.");
        // Deactivate the portal object prefab
        portalPrefab.SetActive(false);
        hasInteracted = false; // Reset the interaction flag
    }
}
