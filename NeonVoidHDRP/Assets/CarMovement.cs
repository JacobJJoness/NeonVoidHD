using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public GameObject[] waypoints; // Array of empty GameObjects representing the destination points
    public float speed = 5f; // The speed of the car

    private int currentWaypointIndex = 0; // Index of the current destination point

    private void Update()
    {
        if (waypoints.Length == 0)
        {
            Debug.LogError("No waypoints assigned to the car!");
            return;
        }

        // Move the car towards the current waypoint
        Transform currentWaypoint = waypoints[currentWaypointIndex].transform;
        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, speed * Time.deltaTime);

        // Check if the car has reached the current waypoint
        if (Vector3.Distance(transform.position, currentWaypoint.position) < 0.1f)
        {
            // Move to the next waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;

            // Check if the car has reached the final waypoint
            if (currentWaypointIndex == 0)
            {
                // Teleport back to the beginning
                transform.position = waypoints[0].transform.position;
            }
        }
    }
}