using UnityEngine;
using UnityEngine.AI;

public class PlayerNavMesh : MonoBehaviour
{
    [SerializeField] private Transform movePositionTransform; // Reference to the checkpoint position
    private NavMeshAgent navMeshAgent;
    private Vector3 startPosition; // To hold the initial start position

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component is missing on the object.");
        }
        // Save the initial start position
        startPosition = transform.position;
        navMeshAgent.stoppingDistance = 0.5f; // Set a suitable stopping distance.
    }

    private void Update()
    {
        if (movePositionTransform != null)
        {
            navMeshAgent.destination = movePositionTransform.position;
            Debug.Log($"Destination Set To: {movePositionTransform.position}, Remaining Distance: {navMeshAgent.remainingDistance}");

            // Check if the agent is within an acceptable range of the destination
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                Debug.Log("Near the Destination, Restarting Player");
                RestartPlayer();
            }
        }
        else
        {
            Debug.LogError("Move Position Transform is not assigned.");
        }
    }

    // Method to restart the player
    private void RestartPlayer()
    {
        // Teleport the player back to the initial start position
        transform.position = startPosition;
        // Reset the navmesh destination
        navMeshAgent.ResetPath();
        Debug.Log("Player restarted at initial start position.");
    }
}
