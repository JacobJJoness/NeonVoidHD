using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitMenu : MonoBehaviour
{
    public GameObject backgroundCanvas; // Reference to the background canvas GameObject
    public GameObject settingsCanvas; // Reference to the settings canvas GameObject

    private bool settingsActive = false; // Flag to track if settings panel is active

    // Start is called before the first frame update
    void Start()
    {
        // Ensure both canvases are initially hidden
        backgroundCanvas.SetActive(false);
        settingsCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player presses the "Escape" key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggle the visibility of both canvases
            settingsActive = !settingsActive;
            backgroundCanvas.SetActive(settingsActive);
            settingsCanvas.SetActive(settingsActive);
        }
    }
}