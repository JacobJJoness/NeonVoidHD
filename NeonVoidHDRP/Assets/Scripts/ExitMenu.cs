using UnityEngine;

public class ExitMenu : MonoBehaviour
{
    public GameObject backgroundCanvas; // Reference to the background canvas GameObject
    public GameObject settingsCanvas; // Reference to the settings canvas GameObject
    public GameObject optionScreen; // Reference to the option screen GameObject

    private void Start()
    {
        // Ensure all UI elements are initially hidden
        backgroundCanvas.SetActive(false);
        settingsCanvas.SetActive(false);
        // Do not automatically deactivate optionScreen here in case you want it to be initially active.

        // Hide the mouse cursor initially
        Cursor.visible = false;
    }

    private void Update()
    {
        // Check if the player presses the "Escape" key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Check if the option screen is active
            if (optionScreen.activeSelf)
            {
                // Deactivate the option screen and other related UI components
                optionScreen.SetActive(false);
                backgroundCanvas.SetActive(false);
                settingsCanvas.SetActive(false);

                // Ensure the cursor is not visible and the mouse cursor is locked
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                // Optionally, resume camera movement or other gameplay elements
                if (CameraManager.Instance != null)
                {
                    CameraManager.Instance.ToggleCameraActive(true);
                }
            }
            else
            {
                // Toggle the visibility of background and settings canvas based on current state
                bool isActive = !backgroundCanvas.activeSelf; // Determine the new active state
                backgroundCanvas.SetActive(isActive);
                settingsCanvas.SetActive(isActive);

                // Adjust the cursor visibility and lock state accordingly
                Cursor.visible = isActive;
                Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;

                // Adjust camera activity based on the UI state
                if (CameraManager.Instance != null)
                {
                    CameraManager.Instance.ToggleCameraActive(!isActive);
                }
            }
        }
    }
}
