using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public Text dialogueText;
    public Button yesButton;
    public Button noButton;
    public Text missionDescriptionText;  // Reference to the text component where mission details are shown

    private string currentMissionName;  // Holds the current mission name

    void Start()
    {
        // Initially hide the dialogue panel
        dialoguePanel.SetActive(false);

        // Setup button listeners
        yesButton.onClick.AddListener(OnYesClicked);
        noButton.onClick.AddListener(OnNoClicked);
    }

    public void ShowDialogue(string text, string missionName)
    {
        currentMissionName = missionName;  // Store the mission name
        dialogueText.text = text;
        dialoguePanel.SetActive(true);
        // Enable cursor and unlock it
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnYesClicked()
    {
        Debug.Log("Player accepted the mission: " + currentMissionName);
        if (missionDescriptionText != null)
        {
            missionDescriptionText.text = "Current Mission: " + currentMissionName;
        }
        else
        {
            Debug.LogError("MissionDescriptionText is not assigned in the DialogueManager.");
        }
        CloseDialogue();
    }

    private void OnNoClicked()
    {
        Debug.Log("Player declined the mission: " + currentMissionName);
        CloseDialogue();
    }

    private void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
        // Hide cursor and lock it again
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
