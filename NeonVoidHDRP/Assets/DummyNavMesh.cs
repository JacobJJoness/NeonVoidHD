using UnityEngine;
using UnityEngine.AI;

public class DummyNavMesh : MonoBehaviour
{
    [SerializeField] private Transform targetPositionTransform; // Reference to the target position
    private NavMeshAgent navMeshAgent;
    private Vector3 startPosition; // To hold the initial start position
    private bool isReturningToStart = false; // State to check if returning to start

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component is missing on the object.");
            return;
        }

        startPosition = transform.position; // Save the starting position
        navMeshAgent.stoppingDistance = 0.5f; // Set a suitable stopping distance

        if (targetPositionTransform != null)
        {
            navMeshAgent.destination = targetPositionTransform.position; // Set initial destination
            Debug.Log("Initial destination set to: " + targetPositionTransform.position);
        }
        else
        {
            Debug.LogError("Target Position Transform is not assigned.");
        }
    }

    void Update()
    {
        if (targetPositionTransform == null)
        {
            Debug.LogError("Target Position Transform is not assigned.");
            return;
        }

        // Check if the agent has reached the current destination
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (!isReturningToStart)
            {
                Debug.Log("Reached Target Position, Returning to Start");
                navMeshAgent.destination = startPosition; // Switch destination to start
                isReturningToStart = true;
            }
            else
            {
                Debug.Log("Returned to Start, Moving to Target Again");
                navMeshAgent.destination = targetPositionTransform.position; // Switch destination to target
                isReturningToStart = false;
            }
        }
    }
}
