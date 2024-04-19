using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetect : MonoBehaviour
{
    public LayerMask groundLayer; // The layer(s) that represent the ground

    private Rigidbody rb; // Reference to the object's Rigidbody component
    private float distanceToGround; // The distance from the object's center to the ground
    private bool isGrounded; // Flag to indicate if the object is grounded

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        distanceToGround = GetComponent<Collider>().bounds.extents.y; // Calculate distance from object's center to the ground
    }

    void Update()
    {
        // Perform a downward raycast to detect the ground
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, distanceToGround + 0.1f, groundLayer))
        {
            isGrounded = true;
            // Adjust the object's position to stay above the ground
            transform.position = new Vector3(transform.position.x, hit.point.y + distanceToGround, transform.position.z);
        }
        else
        {
            isGrounded = false;
        }
    }

    void FixedUpdate()
    {
        // If the object is grounded, freeze its Y velocity to prevent it from falling through the ground
        if (isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        }
    }
}
