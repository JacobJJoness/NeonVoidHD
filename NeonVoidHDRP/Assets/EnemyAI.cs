using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Transform target;
    private EnemyReferences enemyReferences;
    private float shootingDistance;
    private float pathUpdateDeadLine;
    private float lastShootTime;

    private void Awake()
    {
        enemyReferences = GetComponent<EnemyReferences>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        shootingDistance = enemyReferences.navMeshAgent.stoppingDistance;
        lastShootTime = -1f;
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            bool inRange = Mathf.Floor(Vector3.Distance(transform.position, target.position)) <= shootingDistance;
            Debug.Log(Vector3.Distance(transform.position, target.position));
            if(inRange)
            {
                LookAtTarget();

                // Check if enough time has passed since the last shoot
                if (Time.time - lastShootTime >= 1f)
                {
                    ShootAtTarget();
                    lastShootTime = Time.time; // Update the last shoot time
                }
            }
            else
            {
                UpdatePath();
            }

        }
    }


    private void ShootAtTarget()
    {
        Debug.Log("Shoot at player");
        Vector3 shootingPointOffset = new Vector3(0f, 1.5f, 0f);

        Transform shootingPoint = transform;
        // Ensure that the enemy has references to the projectile prefab and shooting point
        if (enemyReferences.projectilePrefab != null && shootingPoint != null)
        {
            // Calculate shoot direction
            Vector3 shootDirection = (target.position) - shootingPoint.position;

            // Ensure that the shoot direction is not purely vertical
            if (shootDirection != Vector3.zero)
            {
                // Normalize the shoot direction
                shootDirection.Normalize();


                // Instantiate a projectile at the shooting point's position and rotation
                GameObject projectile = Instantiate(enemyReferences.projectilePrefab, shootingPoint.position + shootingPointOffset, Quaternion.identity);

                // Optionally, you can apply force or velocity to the projectile
                Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
                if (projectileRb != null)
                {
                    // Apply force towards the player's position
                    projectileRb.AddForce(shootDirection * enemyReferences.projectileForce, ForceMode.Impulse);

                    // Apply rotation towards where the player is/was.
                    projectile.transform.rotation = Quaternion.LookRotation(shootDirection, Vector3.up) * enemyReferences.projectilePrefab.transform.rotation;
                }

                // Destroy projectile after 5 seconds
                Destroy(projectile, 5f);
            }
        }
    }


    private void LookAtTarget()
    {
        Vector3 lookPos = target.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
    }

    private void UpdatePath()
    {
        if(Time.time >= pathUpdateDeadLine)
        {
            //Debug.Log("Updating Enemy Path");
            pathUpdateDeadLine = Time.time + enemyReferences.pathUpdateDelay;
            enemyReferences.navMeshAgent.SetDestination(target.position);
        }
    }
}
