using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public GameObject gunPrefab; // The gun prefab to instantiate when picked up
    public Transform gunHolder; // The transform where the gun will be parented (e.g., hand bone)
    public GameObject pickUpPrompt; // The UI element that prompts the player to pick up the gun

    private GameObject currentGun = null; // The current gun the player is holding
    private GameObject gunInScene = null; // Reference to the interactable gun in the scene
    private bool isNearGun = false; // Flag to check if the player is near a gun

    private InputManager inputManager;

    private void Start()
    {
        inputManager = GetComponent<InputManager>(); // Make sure the InputManager is attached to the same GameObject
        pickUpPrompt.SetActive(false); // Ensure the pickup prompt is hidden by default
    }

    private void Update()
    {
        if (currentGun != null && inputManager.b_Input)
        {
            ShootGun();
        }

        if (currentGun != null && inputManager.drop_Input)
        {
            DropGun();
        }

        if (isNearGun && inputManager.pickUp_Input)
        {
            inputManager.pickUp_Input = false; // Reset the pickUp_Input flag to prevent repeated pickups
            PickUpGun();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Gun"))
        {
            isNearGun = true;
            gunInScene = other.gameObject; // Store reference to the gun object
            pickUpPrompt.SetActive(true); // Show the pickup prompt
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Gun"))
        {
            isNearGun = false;
            gunInScene = null; // Clear the reference when out of range
            pickUpPrompt.SetActive(false); // Hide the pickup prompt
        }
    }

    public void PickUpGun()
    {
        if (currentGun == null && isNearGun && gunInScene != null)
        {
            // Parent the gun in the scene to the gunHolder
            gunInScene.transform.parent = gunHolder;
            gunInScene.transform.localPosition = new Vector3(0.2f, 1.1f, 0.7f);
            gunInScene.transform.localRotation = Quaternion.Euler(0, 0, 0);

            WeaponScript weaponScript = gunInScene.GetComponent<WeaponScript>();
            if (weaponScript != null)
            {
                weaponScript.canShoot = true;
                inputManager.weaponScript = weaponScript;
            }

            // Disable the pickup prompt
            if (pickUpPrompt != null)
                pickUpPrompt.SetActive(false);

            // Set the gun in the scene as the current gun
            currentGun = gunInScene;

            Debug.Log("Gun picked up and configured");
        }
    }


    public void DropGun()
    {
        if (currentGun != null)
        {
            
                
            gunInScene.transform.position = currentGun.transform.position; // Drop at current location

            Destroy(currentGun); // Destroy the instantiated gun
            currentGun = null;

            Debug.Log("Gun dropped.");
        }
    }

    private void ShootGun()
    {
        Debug.Log("Shooting gun");
    }
}
