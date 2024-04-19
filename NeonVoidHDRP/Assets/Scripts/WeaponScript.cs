using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public Transform firePoint; // Where the bullets are shot from
    public GameObject bulletPrefab; // The bullet prefab
    public float bulletSpeed = 20f; // Speed at which bullets will be fired
    public float fireRate = 0.5f; // Time between each shot
    private float nextFireTime = 0f; // Time when the next shot can be fired

    public bool canShoot = false; // Only true when the gun is picked up

    public GameObject gunCameraPrefab; // Prefab of the gun camera
    public GameObject cameraManagerPrefab; // Prefab of the camera manager
    public int layerType = 0; // Default layer is 'Default'

    private GameObject gunCameraInstance; // Instance of the gun camera
    private GameObject cameraManagerInstance; // Instance of the camera manager

    private Camera mainCamera; // Reference to the main camera
    private bool isSwitchingCamera = false; // Flag to track if camera switching is in progress
    private Vector2 cameraRotation; // Stores current rotation of the camera
    public float cameraSensitivity = 100f; // Sensitivity of camera rotation

    private void Start()
    {
        mainCamera = Camera.main;
        gameObject.layer = layerType; // Set the layer of the gun

        // Instantiate and disable the gun camera at the start
        if (gunCameraPrefab != null)
        {
            gunCameraInstance = Instantiate(gunCameraPrefab, firePoint.position, Quaternion.LookRotation(firePoint.forward));
            gunCameraInstance.transform.SetParent(firePoint); // Attach camera to fire point
            gunCameraInstance.transform.localPosition = new Vector3(0, 0.3f, -1.35f); // Position slightly above and behind the fire point
            gunCameraInstance.SetActive(false);
            gunCameraInstance.layer = layerType; // Set the camera to the same layer as the gun
        }
    }

    private void Update()
    {
        // Check if the player is holding the gun and holding down the right mouse button to activate the gun camera
        if (canShoot && Input.GetMouseButtonDown(1))
        {
            SwitchToGunCamera();
        }
        else if (canShoot && Input.GetMouseButtonUp(1))
        {
            SwitchToMainCamera();
        }

        // Rotate the gun camera with the mouse if it is active and the player is holding the gun
        if (canShoot && gunCameraInstance != null && gunCameraInstance.activeSelf)
        {
            RotateGunCamera();
        }
    }

    private void RotateGunCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;

        // Adjust the camera rotation based on mouse X input only for horizontal movement
        cameraRotation.x += mouseX;

        // Apply rotation to the camera horizontally
        gunCameraInstance.transform.localRotation = Quaternion.Euler(0, cameraRotation.x, 0);
    }

    public void Shoot()
    {
        if (!canShoot || Time.time < nextFireTime)
        {
            Debug.Log("Cannot shoot: either not ready or cooldown.");
            return;
        }

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.layer = layerType; // Set the layer of the bullet
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(firePoint.forward * bulletSpeed, ForceMode.Impulse);
            Destroy(bullet, 3f); // Destroy bullet after some time
            nextFireTime = Time.time + fireRate;
        }
        else
        {
            Debug.LogError("No Rigidbody found on the bullet prefab!");
        }
    }

    private void SwitchToGunCamera()
    {
        if (!isSwitchingCamera && gunCameraInstance != null)
        {
            isSwitchingCamera = true;
            if (cameraManagerInstance == null)
            {
                cameraManagerInstance = Instantiate(cameraManagerPrefab);
                cameraManagerInstance.layer = layerType; // Set the layer of the camera manager
            }

            cameraManagerInstance.SetActive(false);
            gunCameraInstance.SetActive(true);
            mainCamera.enabled = false;
            isSwitchingCamera = false;
        }
    }

    private void SwitchToMainCamera()
    {
        if (!isSwitchingCamera && mainCamera != null)
        {
            isSwitchingCamera = true;
            mainCamera.enabled = true;
            if (gunCameraInstance != null)
            {
                gunCameraInstance.SetActive(false);
            }
            isSwitchingCamera = false;
        }
    }
}
