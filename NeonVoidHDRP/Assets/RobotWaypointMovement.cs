using UnityEngine;

public class RobotWaypointMovement : MonoBehaviour
{
    public Transform[] waypoints; // Array of destination points
    public float speed = 5f; // The speed of the robot
    public Animator robotAnimator; // Reference to the robot's animator
    public string walkAnimationName = "Walk"; // Name of the walk animation in the animator controller
    public LayerMask groundLayer; // Layer mask for the ground

    private int currentWaypointIndex = 0; // Index of the current destination point
    private bool isWalking = false; // Flag to control whether the robot is walking

    private Rigidbody rb;
    private CapsuleCollider collider;
    private bool isGrounded = false; // Flag to check if the robot is grounded

    private void Start()
    {
        // Add Rigidbody component if not already attached
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = false; // Set to false to allow physics interactions
        }

        // Add Capsule Collider component if not already attached
        collider = GetComponent<CapsuleCollider>();
        if (collider == null)
        {
            collider = gameObject.AddComponent<CapsuleCollider>();
            // Adjust the collider size and center as needed
            collider.center = new Vector3(0f, 0.5f, 0f);
            collider.height = 1.0f;
            collider.radius = 0.25f;
        }
    }

    private void Update()
    {
        if (waypoints.Length == 0)
        {
            Debug.LogError("No waypoints assigned to the robot!");
            return;
        }

        MoveToWaypoint();

        // Check if the robot is grounded
        isGrounded = Physics.Raycast(transform.position, -transform.up, collider.height / 2 + 0.1f, groundLayer);
    }

    private void MoveToWaypoint()
    {
        if (!isWalking)
            return;

        // Get the direction to the next waypoint
        Vector3 direction = (waypoints[currentWaypointIndex].position - transform.position).normalized;
        direction.y = 0; // Ignore vertical direction

        // Move the robot towards the current waypoint
        transform.position += direction * speed * Time.deltaTime;

        // Rotate towards the next waypoint
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }

        // Check if the robot has reached the current waypoint
        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
        {
            // Stop playing the walk animation
            if (robotAnimator != null)
            {
                robotAnimator.SetBool(walkAnimationName, false);
            }

            // Move to the next waypoint
            currentWaypointIndex++;

            // Check if the robot has reached the final waypoint
            if (currentWaypointIndex >= waypoints.Length)
            {
                // Teleport back to the beginning
                currentWaypointIndex = 0;
                transform.position = waypoints[0].position;
            }
        }
    }


    // Method to start or stop the robot's walking animation
    public void SetWalking(bool walking)
    {
        isWalking = walking;

        // Update the walk animation state
        if (robotAnimator != null)
        {
            robotAnimator.SetBool(walkAnimationName, walking);
        }
    }
}
