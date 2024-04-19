using UnityEngine;

public class ExitMenu : MonoBehaviour
{
    public GameObject backgroundCanvas; // Reference to the background canvas GameObject
    public GameObject settingsCanvas; // Reference to the settings canvas GameObject

    private bool settingsActive = false; // Flag to track if settings panel is active

    void Start()
    {
        // Ensure both canvases are initially hidden
        backgroundCanvas.SetActive(false);
        settingsCanvas.SetActive(false);

        // Hide the mouse cursor initially
        Cursor.visible = false;
    }

    void Update()
    {
        // Check if the player presses the "Escape" key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggle the visibility of both canvases
            settingsActive = !settingsActive;
            backgroundCanvas.SetActive(settingsActive);
            settingsCanvas.SetActive(settingsActive);

            // Toggle the visibility of the mouse cursor
            Cursor.visible = settingsActive;

            // Lock or unlock the mouse cursor based on settingsActive
            Cursor.lockState = settingsActive ? CursorLockMode.None : CursorLockMode.Locked;

            // Pause or resume camera movement by controlling it through CameraManager
            if (CameraManager.Instance != null)
            {
                CameraManager.Instance.ToggleCameraActive(!settingsActive);
            }
        }
    }
}
