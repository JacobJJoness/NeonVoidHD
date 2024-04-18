using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public int numBulletPieces = 10; // Number of smaller bullet pieces to spawn
    public float explosionForce = 100f; // Force of the explosion
    public GameObject bulletPiecePrefab; // Prefab of the smaller bullet pieces

    void Start()
    {
        Explode();
    }

    void Explode()
    {
        // Apply explosion force to each bullet piece
        for (int i = 0; i < numBulletPieces; i++)
        {
            GameObject bulletPiece = Instantiate(bulletPiecePrefab, transform.position, Quaternion.identity);

            // Apply force to each bullet piece in random directions
            Rigidbody rb = bulletPiece.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 randomDirection = Random.insideUnitSphere.normalized;
                rb.AddForce(randomDirection * explosionForce, ForceMode.Impulse);
            }
        }

        // Destroy the explosion object
        Destroy(gameObject);
    }
}
