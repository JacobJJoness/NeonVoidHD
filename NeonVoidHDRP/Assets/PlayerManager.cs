using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    CameraManager cameraManager;
    Animator animator;
    PlayerMovement playerMovement;

    public bool isInteracting;
    public bool isUsingRootMotion;

    // Singleton pattern to ensure only one instance of the player exists
    public static PlayerManager Instance { get; private set; }

    private void Awake()
    {
        // If an instance already exists and it isn't this one, destroy this one
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            // This is either the first instance or the same as the current one, so make it the Singleton
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Make the player persist across scenes
        }

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
