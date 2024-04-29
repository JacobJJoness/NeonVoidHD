using UnityEngine;

public class NPCTalk : MonoBehaviour
{
    public GameObject portalPrefab;  // Assuming this is for other purposes too
    public DialogueCanvasController dialogueCanvasController;  // Reference to the dialogue controller
    public string offeredMissionName;  // Name of the mission this NPC will offer
    private bool isPlayerInside = false;
    private bool hasInteracted = false;

    void Start()
    {
        // Subscribe to the mission accepted event
        if (dialogueCanvasController != null)
        {
            dialogueCanvasController.OnMissionAccepted += HandleMissionAccepted;
        }
        else
        {
            Debug.LogError("DialogueCanvasController is not assigned in NPCTalk.");
        }
    }

    private void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(KeyCode.E) && !hasInteracted)
        {
            // Check if all missions are completed before showing new mission dialog
            if (MissionManager.Instance.AreAllMissionsComplete())
            {
                ShowMissionDialogue();
            }
            else
            {
                Debug.Log("Complete all current missions before proceeding.");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered NPC's trigger zone.");
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited NPC's trigger zone.");
            isPlayerInside = false;
            hasInteracted = false; // Reset interaction flag when player exits the zone
        }
    }

    private void ShowMissionDialogue()
    {
        if (!hasInteracted)
        {
            Debug.Log("Player interacts with NPC to discuss mission.");
            Mission mission = new Mission(offeredMissionName);
            dialogueCanvasController.ShowMission(mission);
            hasInteracted = true;
        }
    }

    private void HandleMissionAccepted(bool accepted)
    {
        if (accepted)
        {
            portalPrefab.SetActive(true);
            // Optionally reset hasInteracted to allow new interactions
            hasInteracted = false;
        }
    }

    void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        if (dialogueCanvasController != null)
        {
            dialogueCanvasController.OnMissionAccepted -= HandleMissionAccepted;
        }
    }
}
