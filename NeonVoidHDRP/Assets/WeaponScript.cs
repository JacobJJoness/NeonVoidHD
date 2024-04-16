using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public Transform firePoint; // Where the bullets are shot from
    public GameObject bulletPrefab; // The bullet prefab
    public float bulletSpeed = 20f; // Speed at which bullets will be fired

    public bool canShoot = false; // Only true when the gun is picked up

    // Removed Update() and input check; Shooting is now triggered by the InputManager

    // Shoot method is now explicitly public and will be called by InputManager
    public void Shoot()
    {
        if (!canShoot)
        {
            return;  // If the weapon cannot shoot, just return and do nothing
        }

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(firePoint.forward * bulletSpeed, ForceMode.Impulse);
        }
        Destroy(bullet, 3f); // Destroy bullet after 3 seconds to clean up
    }
}
