using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private Transform target;
    private EnemyReferences enemyReferences;
    private float shootingDistance = 5f;
    private float punchingDistance = 2f;  // Distance within which the enemy will punch
    private float lastShootTime;
    private float pathUpdateInterval;

    private Animator animator;  // Reference to the Animator component

    private void Awake()
    {
        enemyReferences = GetComponent<EnemyReferences>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        pathUpdateInterval = enemyReferences.pathUpdateDelay;
        animator = GetComponent<Animator>();  // Get the Animator component
    }

    void Start()
    {
        shootingDistance = enemyReferences.navMeshAgent.stoppingDistance;
        lastShootTime = Time.time;
    }

    void Update()
    {
        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            // Check if the enemy should punch
            if (distanceToTarget <= punchingDistance)
            {
                PunchTarget();
            }
            else if (distanceToTarget <= shootingDistance && distanceToTarget > punchingDistance)
            {
                LookAtTarget();
                if (Time.time - lastShootTime >= 1f)
                {
                    ShootAtTarget();
                    lastShootTime = Time.time;
                }
            }
            else if (Time.time >= lastShootTime + pathUpdateInterval)
            {
                UpdatePath();
                lastShootTime = Time.time;  // Using the same time tracking for shooting and path updates for simplicity
            }
        }
    }

    private void PunchTarget()
    {
        Debug.Log("Enemy punching player!");

        // Trigger the punch animation
        animator.SetTrigger("EnemyAttack");

        // Here you could also apply damage to the player if you're handling health reduction on punch
        // For example, assuming PlayerHealth script is attached to the player
        PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(10);  // Assume punching deals 10 damage
        }
    }

    private void ShootAtTarget()
    {
        Vector3 shootingPointOffset = new Vector3(0f, 1.5f, 0f);
        Vector3 shootDirection = (target.position - transform.position).normalized;

        if (enemyReferences.projectilePrefab != null)
        {
            GameObject projectile = Instantiate(
                enemyReferences.projectilePrefab,
                transform.position + shootingPointOffset,
                Quaternion.LookRotation(shootDirection)
            );

            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
            if (projectileRb != null)
            {
                projectileRb.AddForce(shootDirection * enemyReferences.projectileForce, ForceMode.Impulse);
            }

            Destroy(projectile, 5f);  // Adjust time as needed for your game design
        }
    }

    private void LookAtTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }

    private void UpdatePath()
    {
        enemyReferences.navMeshAgent.SetDestination(target.position);
    }
}
