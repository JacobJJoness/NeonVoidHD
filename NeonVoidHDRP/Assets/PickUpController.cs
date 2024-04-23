using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public GameObject gunPrefab; // The gun prefab to instantiate when picked up
    public Transform gunHolder; // The transform where the gun will be parented (e.g., hand bone)
    public GameObject pickUpPrompt; // The UI element that prompts the player to pick up the gun
    public Animator animator;
    private GameObject currentGun = null; // The current gun the player is holding
    private GameObject gunInScene = null; // Reference to the interactable gun in the scene
    private bool isNearGun = false; // Flag to check if the player is near a gun

    private InputManager inputManager;

    public bool IsHoldingGun { get; private set; } = false;

    public GameObject playerGun;

    private void Start()
    {
        inputManager = GetComponent<InputManager>(); // Make sure the InputManager is attached to the same GameObject
        pickUpPrompt.SetActive(false); // Ensure the pickup prompt is hidden by default
        animator = GetComponent<Animator>();
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

        // Ensure pickup is only triggered by the correct input and conditions
        if (isNearGun && inputManager.pickUp_Input)
        {
            inputManager.pickUp_Input = false; // Prevent continuous pickup on holding the key
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
            Debug.Log("Player is near a gun.");
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
            Debug.Log("Picking up gun now.");

            // Activate the player's gun object
            if (playerGun != null)
            {
                playerGun.SetActive(true);  // Activate the player gun
                currentGun = playerGun;  // Set the current gun reference

                // Ensure the gun can shoot
                WeaponScript weaponScript = currentGun.GetComponent<WeaponScript>();
                if (weaponScript != null)
                {
                    weaponScript.canShoot = true;  // Enable shooting
                    inputManager.weaponScript = weaponScript;  // Assign the script to the input manager if necessary
                }
                else
                {
                    Debug.LogError("WeaponScript missing from the player gun.");
                }
            }
            else
            {
                Debug.LogError("Player gun is not assigned in the Inspector");
            }

            IsHoldingGun = true;

            if (animator != null)
            {
                animator.SetBool("isHoldingGun", true);
            }

            // Optionally deactivate the gun in the scene
            gunInScene.SetActive(false);
            Debug.Log("Gun in scene deactivated, and player gun activated.");
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

            // Instantiate a copy of the gun in the scene at the specified drop position
            Vector3 dropPosition = new Vector3(gunHolder.position.x, gunHolder.position.y + 0.5f, gunHolder.position.z);
            GameObject droppedGun = Instantiate(gunPrefab, dropPosition, Quaternion.identity);

            // Optionally disable any shooting capability initially or reset other settings
            WeaponScript weaponScript = droppedGun.GetComponent<WeaponScript>();
            if (weaponScript != null)
            {
                weaponScript.canShoot = false; // Ensure the dropped gun cannot shoot
            }

            if (animator != null)
            {
                animator.SetBool("isHoldingGun", false);
            }

            // Deactivate the current gun (held by the player)
            currentGun.SetActive(false);

            // Also ensure no residual shooting capabilities
            WeaponScript currentGunScript = currentGun.GetComponent<WeaponScript>();
            if (currentGunScript != null)
            {
                currentGunScript.canShoot = false; // Disable shooting capability of the gun being held
            }

            IsHoldingGun = false;

            // Ensure there is a Collider for interaction, set it to active
            Collider gunCollider = droppedGun.GetComponent<Collider>();
            if (gunCollider == null)
            {
                gunCollider = droppedGun.AddComponent<BoxCollider>(); // Add a collider if not present
            }
            gunCollider.enabled = true;

            // Log the drop action
            Debug.Log("Gun dropped at position: " + dropPosition);

            // Clear the currentGun reference to prevent further interactions with it
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
