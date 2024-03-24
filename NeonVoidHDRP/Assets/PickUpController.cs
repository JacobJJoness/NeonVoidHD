using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public GameObject gunPrefab; // The gun prefab to instantiate when picked up
    public Transform gunHolder; // The transform where the gun will be parented (e.g., hand bone)
    public GameObject pickUpPrompt; // The UI element that prompts the player to pick up the gun

    private GameObject currentGun = null; // The current gun the player is holding
    private bool isNearGun = false; // Flag to check if the player is near a gun

    // Reference to the InputManager to check for inputs
    private InputManager inputManager;

    private void Start()
    {
        inputManager = GetComponent<InputManager>(); // Make sure the InputManager is attached to the same GameObject
        pickUpPrompt.SetActive(false); // Ensure the pickup prompt is hidden by default
    }

    private void Update()
    {
        // Check for shooting input if the player has a gun
        if (currentGun != null && inputManager.b_Input) // Assuming 'B' button is for shooting
        {
            ShootGun();
        }

        // Check for pickup input when near a gun
        if (isNearGun && inputManager.pickUp_Input)
        {
            inputManager.pickUp_Input = false; // Reset the pickUp_Input flag to prevent repeated pickups
            PickUpGun();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called with: " + other.gameObject.name);
        if (other.gameObject.CompareTag("Gun")) // Check if the player is near a gun
        {
            isNearGun = true;
            pickUpPrompt.SetActive(true); // Show the pickup prompt
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Gun")) // Check if the player leaves the vicinity of a gun
        {
            isNearGun = false;
            pickUpPrompt.SetActive(false); // Hide the pickup prompt
        }
    }

    public void PickUpGun()
    {
        Debug.Log("PickUpGun called");
        if (currentGun == null && isNearGun) // Ensure the player doesn't already have a gun and is near one
        {
            
            // Instantiate the gun prefab and parent it to the gunHolder
            currentGun = Instantiate(gunPrefab, gunHolder.position, gunHolder.rotation, gunHolder);

            // After instantiation, you might need to fine-tune the position and rotation
            currentGun.transform.localPosition = new Vector3(0.2f, 1.1f, 0.7f); // Adjust these values as needed
            currentGun.transform.localRotation = Quaternion.Euler(0, 0, 0); // Adjust these values as needed

            // Disable the collider or set it to a trigger to avoid physical interactions
            Collider gunCollider = currentGun.GetComponent<Collider>();
            if (gunCollider != null)
            {
                gunCollider.enabled = false; // Disable the collider
                                             // Or gunCollider.isTrigger = true; // Set it as a trigger
            }
            Debug.Log("Move Gun to Player");

            // Hide the pickup prompt since the gun has been picked up
            pickUpPrompt.SetActive(false);
        }
    }

    private void ShootGun()
    {
        // Implement shooting logic here (e.g., animating the gun, playing a sound, firing bullets)
        Debug.Log("Shooting gun");
    }
}
