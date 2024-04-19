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
            gunInScene.transform.SetParent(gunHolder);
            gunInScene.transform.localPosition = new Vector3(0.2f, 1.1f, 0.7f);
            gunInScene.transform.localRotation = Quaternion.identity; // Simplify rotation setting

            // Get the WeaponScript component from the gun
            WeaponScript weaponScript = gunInScene.GetComponent<WeaponScript>();
            if (weaponScript != null)
            {
                weaponScript.canShoot = true;
                inputManager.weaponScript = weaponScript;
            }
            else
            {
                Debug.LogError("WeaponScript missing from the gun prefab.");
            }

            // Disable the pickup prompt, if it exists
            if (pickUpPrompt != null)
            {
                pickUpPrompt.SetActive(false);
            }

            // Set the gun in the scene as the current gun
            currentGun = gunInScene;
            Debug.Log("Gun picked up and configured.");
        }
        else
        {
            Debug.LogWarning("Pick up conditions not met or gun already in hand.");
        }
    }


    public void DropGun()
    {
        if (currentGun != null)
        {
            Debug.Log("Dropping gun: " + currentGun.name);

            // Unparent the gun from the gun holder and ensure it remains active
            currentGun.SetActive(true);
            currentGun.transform.SetParent(null);

            // Position the gun at an adjusted height to prevent it from going into the ground
            Vector3 dropPosition = new Vector3(gunHolder.position.x, gunHolder.position.y + 0.5f, gunHolder.position.z);
            currentGun.transform.position = dropPosition;

            // Disable the gun's shooting capability
            WeaponScript weaponScript = currentGun.GetComponent<WeaponScript>();
            if (weaponScript != null)
            {
                weaponScript.canShoot = false;
            }
            else
            {
                Debug.LogError("WeaponScript missing from the gun being dropped.");
            }

            // Ensure there is a Collider for interaction, adding one if necessary
            Collider gunCollider = currentGun.GetComponent<Collider>();
            if (gunCollider == null)
            {
                gunCollider = currentGun.AddComponent<BoxCollider>();
            }
            gunCollider.enabled = true;

            // Log the drop location and clear the reference to currentGun
            Debug.Log("Gun dropped at position: " + dropPosition);
            currentGun = null;
        }
        else
        {
            Debug.LogWarning("No gun to drop.");
        }
    }





    private void ShootGun()
    {
        if (currentGun != null)
        {
            Debug.Log("Attempting to shoot gun.");
            WeaponScript weaponScript = currentGun.GetComponent<WeaponScript>();
            if (weaponScript != null && weaponScript.canShoot)
            {
                // Call weapon script shoot method or handle shooting logic here
                Debug.Log("Gun fired.");
            }
            else
            {
                Debug.Log("Gun cannot shoot or script missing.");
            }
        }
        else
        {
            Debug.Log("No gun to shoot.");
        }
    }

}
