using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if collided with something other than the shooter
        if (collision.gameObject != gameObject && !collision.gameObject.CompareTag("Enemy"))
        {
            // Destroy the projectile
            Destroy(gameObject);
        }
    }
}
