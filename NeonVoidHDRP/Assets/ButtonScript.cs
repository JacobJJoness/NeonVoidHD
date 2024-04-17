using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonScript : MonoBehaviour, IPointerClickHandler
{
    public RectTransform imageTransform; // Reference to the RectTransform of the image you want to zoom
    public float zoomScale = 1.5f; // Amount to scale the image by when clicked
    public float zoomDuration = 0.2f; // Duration of the zoom animation
    public Image blackOverlay; // Reference to the Image component for the black overlay
    public float overlayAlpha = 0.5f; // Alpha value of the black overlay

    private Vector3 originalScale; // Original scale of the image

    // Start is called before the first frame update
    void Start()
    {
        // Store the original scale of the image
        originalScale = imageTransform.localScale;

        // Hide the black overlay initially
        blackOverlay.gameObject.SetActive(false);
    }

    // Called when the button is clicked
    public void OnPointerClick(PointerEventData eventData)
    {
        // Show the black overlay
        blackOverlay.gameObject.SetActive(true);

        // Start a coroutine to perform the zoom animation
        StartCoroutine(ZoomAnimation());
    }

    // Coroutine to perform the zoom animation
    private IEnumerator ZoomAnimation()
    {
        // Calculate the target scale
        Vector3 targetScale = originalScale * zoomScale;

        // Interpolate the scale of the image over time
        float elapsedTime = 0f;
        while (elapsedTime < zoomDuration)
        {
            // Calculate the interpolation factor (0 to 1)
            float t = elapsedTime / zoomDuration;

            // Interpolate the scale
            imageTransform.localScale = Vector3.Lerp(originalScale, targetScale, t);

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the image reaches the exact target scale
        imageTransform.localScale = targetScale;

        // Hide the black overlay
        blackOverlay.gameObject.SetActive(false);
    }
}
