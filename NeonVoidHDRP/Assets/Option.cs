using UnityEngine;

public class OptionButton : MonoBehaviour
{
    public GameObject optionScreen;
    public GameObject settingsCanvas;

    public void OpenOptionScreen()
    {
        // Turn on the OptionScreen canvas
        optionScreen.SetActive(true);

        // Turn off the Settings Canvas
        settingsCanvas.SetActive(false);

        // Debug log to verify if the method is being called
        Debug.Log("Option screen opened!");
    }

    public void GoBackToSettings()
    {
        // Turn off the OptionScreen canvas
        optionScreen.SetActive(false);

        // Turn on the Settings Canvas
        settingsCanvas.SetActive(true);

        // Debug log to verify if the method is being called
        Debug.Log("Returned to settings screen!");
    }
}
