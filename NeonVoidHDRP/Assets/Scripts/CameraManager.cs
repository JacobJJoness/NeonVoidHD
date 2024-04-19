using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    public Transform targetTransform; // The object the camera will follow
    public Transform cameraPivot; // The object the camera uses to pivot
    public Transform cameraTransform; // The transform of the actual camera object in the scene
    public LayerMask collisionLayers; // The layers we want our camera to collide with

    private InputManager inputManager; // Reference to the InputManager

    private Vector3 cameraFollowVelocity = Vector3.zero;
    private Vector3 cameraVectorPosition;

    public float cameraCollisionOffset = 0.2f; // How much the camera will jump off objects it collides with 
    public float minimumCollisionOffset = 0.2f;
    public float cameraCollisionRadius = 2;
    public float cameraFollowSpeed = 0.2f;
    public float cameraLookSpeed = 2;
    public float cameraPivotSpeed = 2;

    public float lookAngle; // Camera looking up and down
    public float pivotAngle; // Camera looking left and right
    public float minimumPivotAngle = -35;
    public float maximumPivotAngle = 35;

    private Camera mainCamera;
    private bool cameraIsActive = true; // Control camera activity

    private void Awake()
    {
        // Initialize singleton instance
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        // Locking cursor in the middle of the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Initialize the input manager reference
        inputManager = FindObjectOfType<InputManager>();

        // Set the default camera transform if not set
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        // Fetching targetTransform from PlayerManager if needed
        if (targetTransform == null)
            targetTransform = FindObjectOfType<PlayerManager>().transform;
    }

    private void Update()
    {
        if (cameraIsActive)
        {
            HandleAllCameraMovement();
        }
    }

    public void ToggleCameraActive(bool isActive)
    {
        cameraIsActive = isActive;
    }

    public void HandleAllCameraMovement()
    {
        if (!cameraIsActive) return;  // Ensure no movement if deactivated

        FollowTarget();
        RotateCamera();
        HandleCameraCollisions();
    }

    private void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref cameraFollowVelocity, cameraFollowSpeed);
        transform.position = targetPosition;
    }

    private void RotateCamera()
    {
        Vector3 rotation;
        Quaternion targetRotation;

        lookAngle += (inputManager.cameraInputX * cameraLookSpeed);
        pivotAngle -= (inputManager.cameraInputY * cameraPivotSpeed);
        pivotAngle = Mathf.Clamp(pivotAngle, minimumPivotAngle, maximumPivotAngle);

        rotation = Vector3.zero;
        rotation.y = lookAngle;
        targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }

    private void HandleCameraCollisions()
    {
        float targetPosition = cameraTransform.localPosition.z;

        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivot.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPosition), collisionLayers))
        {
            Debug.Log("Wall detected: " + hit.collider.gameObject.name);
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosition = -(distance - cameraCollisionOffset);
        }

        if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
        {
            targetPosition -= minimumCollisionOffset;
        }

        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, Time.deltaTime * 10);
        cameraTransform.localPosition = cameraVectorPosition;
    }

    public void ChangeCamera(Transform newCameraTransform)
    {
        // Change the main camera's position and rotation to the new transform
        mainCamera.transform.position = newCameraTransform.position;
        mainCamera.transform.rotation = newCameraTransform.rotation;
    }

    public void ChangeTarget(Transform newTarget)
    {
        targetTransform = newTarget;
    }

}
