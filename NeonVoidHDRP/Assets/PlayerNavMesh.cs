using UnityEngine;
using UnityEngine.AI;

public class PlayerNavMesh : MonoBehaviour
{
    [SerializeField] private Transform movePositionTransform; // Reference to the checkpoint position
    private NavMeshAgent navMeshAgent;
    private Vector3 startPosition; // To hold the initial start position
    private bool returningToStart = false; // State to check if returning to start

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component is missing on the object.");
        }
        startPosition = transform.position;
        navMeshAgent.stoppingDistance = 0.5f; // Set a suitable stopping distance.
    }

    private void Update()
    {
        if (movePositionTransform != null)
        {
            if (!returningToStart)
            {
                navMeshAgent.destination = movePositionTransform.position;
                Debug.Log($"Destination Set To: {movePositionTransform.position}, Remaining Distance: {navMeshAgent.remainingDistance}");
            }

            // Check if the agent is within an acceptable range of the destination
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (!returningToStart)
                {
                    Debug.Log("Near the Destination, Returning to Start");
                    returningToStart = true;
                    navMeshAgent.destination = startPosition;
                }
                else
                {
                    Debug.Log("Returned to Start, Moving to Target Again");
                    returningToStart = false;
                    navMeshAgent.destination = movePositionTransform.position;
                }
            }
        }
        else
        {
            Debug.LogError("Move Position Transform is not assigned.");
        }
    }
}
