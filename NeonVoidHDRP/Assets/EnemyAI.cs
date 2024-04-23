using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform firePoint;  // The point from which bullets are fired
    public float fireRate = 1f;  // Time between shots in seconds
    public float projectileSpeed = 30f;  // Speed at which projectiles are fired

    private Transform target;
    private EnemyReferences enemyReferences;
    private float shootingDistance = 5f;
    private float punchingDistance = 2f;
    private float lastShootTime;
    private float lastPunchTime;
    private float punchCooldown = 2f;  // Cooldown time in seconds between punches

    private Animator animator;  // Reference to the Animator component

    private void Awake()
    {
        enemyReferences = GetComponent<EnemyReferences>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        shootingDistance = enemyReferences.navMeshAgent.stoppingDistance;
        lastShootTime = -fireRate;  // Ensures that the enemy can shoot immediately after the game starts
        lastPunchTime = Time.time - punchCooldown;  // Initialize to allow immediate punching at start
    }

    void Update()
    {
        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (distanceToTarget <= punchingDistance && Time.time - lastPunchTime >= punchCooldown)
            {
                PunchTarget();
                lastPunchTime = Time.time;
            }
            else if (distanceToTarget <= shootingDistance && distanceToTarget > punchingDistance)
            {
                LookAtTarget();
                if (Time.time - lastShootTime >= fireRate)
                {
                    ShootAtTarget();
                    lastShootTime = Time.time;
                }
            }
        }
    }

    private void PunchTarget()
    {
        Debug.Log("Enemy punching player!");
        animator.SetTrigger("EnemyAttack");
        PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(10);
        }
    }

    private void ShootAtTarget()
    {
        if (enemyReferences.projectilePrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(
                enemyReferences.projectilePrefab,
                firePoint.position,
                Quaternion.LookRotation((target.position - firePoint.position).normalized)
            );

            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
            if (projectileRb != null)
            {
                projectileRb.AddForce((target.position - firePoint.position).normalized * projectileSpeed, ForceMode.Impulse);
            }

            Destroy(projectile, 5f);  // Projectiles destroy after 5 seconds to clean up the scene
        }
    }

    private void LookAtTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }
}
