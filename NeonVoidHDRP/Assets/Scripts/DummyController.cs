using UnityEngine;

public class DummyController : MonoBehaviour
{
    private Animator animator;
    private GameObject player;
    private float triggerDistance = 5.0f; // Distance to trigger push
    private bool playerInRange = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");

        // Initialize animator state to ensure it starts in the Default State
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("DefaultState") == false)
        {
            animator.Play("DefaultState");
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);
        bool currentlyInRange = distance < triggerDistance;

        // Check if there's a change in range status
        if (currentlyInRange && !playerInRange)
        {
            animator.SetTrigger("GetPushed");
            playerInRange = true; // Update flag
        }
        else if (!currentlyInRange && playerInRange)
        {
            animator.ResetTrigger("GetPushed");
            animator.Play("DefaultState");
            playerInRange = false; // Update flag
        }
    }
}
