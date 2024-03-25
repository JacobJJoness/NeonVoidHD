using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    CameraManager cameraManager;
    Animator animator;
    PlayerMovement playerMovement;

    public bool isInteracting;
    public bool isUsingRootMotion;

    private void Awake()
    {
        // Initialize player components
        animator = GetComponent<Animator>();
        inputManager = GetComponentInChildren<InputManager>();
        cameraManager = FindObjectOfType<CameraManager>();
        playerMovement = GetComponentInChildren<PlayerMovement>();
    }

    private void Update()
    {
        inputManager.HandleAllInputs();
    }

    private void FixedUpdate()
    {
        playerMovement.HandleAllMovement();
    }

    private void LateUpdate()
    {
        cameraManager.HandleAllCameraMovement();

        // Update states based on the animator
        isInteracting = animator.GetBool("isInteracting");
        isUsingRootMotion = animator.GetBool("isUsingRootMotion");
        playerMovement.isJumping = animator.GetBool("isJumping");
        animator.SetBool("isGrounded", playerMovement.isGrounded);
    }
}
