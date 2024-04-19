using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerManager playerManager;

    AnimatorManager animatorManager;

    InputManager inputManager;

    Vector3 moveDirection;

    Transform cameraObject;

    public Rigidbody playerRigidbody;

    private bool isDashing;
    private float dashTimer;
    private Vector3 dashDirection;

    [Header("Falling")]
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;

    public float rayCastHeightOffSet = 0.5f;
    public LayerMask groundLayer;

    [Header("Movement Flags")]
    public bool isSprinting;
    public bool isGrounded;
    public bool isJumping;

    [Header("Movement Speeds")]
    public float walkingSpeed = 1.5f;

    public float runningSpeed = 5;

    public float sprintingSpeed = 7;

    public float rotationSpeed = 15;

    [Header("Jump Speeds")]
    public float jumpHeight = 3;
    public float gravityIntensity = -15;


    [Header("Dash Speeds")]
    public float dashDistance = 5;
    public float dashDuration = 1;

    public bool isPunching = false;

    //Awake is used instead of start to ensure that the script is loaded before any other script
    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponent<AnimatorManager>();
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
    }

    // Update is called once per frame, acting as the Update() method in the PlayerManager script
    public void HandleAllMovement()
    {
        HandleFallingAndLanding();

        if (playerManager.isInteracting)
        {
            return;
        }

        HandleMovement();
        HandleRotation();
        HandleDash();
        HandlePunch();
    }

    private void HandlePunch()
    {
        if (inputManager.punch_Input)
        {
            inputManager.punch_Input = false;
            Debug.Log("Punch input received.");

            isPunching = true; // Set isPunching to true when the punch starts
            animatorManager.PlayTargetAnimation("Punch", true);

            // Optionally, use a coroutine to reset isPunching if you don't want to use animation events
            StartCoroutine(ResetPunching());
        }
    }

    IEnumerator ResetPunching()
    {
        yield return new WaitForSeconds(0.5f); // Adjust this duration to match your punch animation duration
        isPunching = false; // Reset isPunching when the animation is likely done
    }

    //WASD Movement and Sprinting
    private void HandleMovement()
    {
        if (isJumping)
            return;

        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (isSprinting)
        {
            moveDirection = moveDirection * sprintingSpeed;
        }
        else
        {
            if (inputManager.moveAmount >= 0.5f)
            {
                moveDirection = moveDirection * runningSpeed;
            }
            else
            {
                moveDirection = moveDirection * walkingSpeed;
            }
        }

        Vector3 movementVelocity = moveDirection;
        playerRigidbody.velocity = movementVelocity;

        if (inputManager.horizontalInput != 0 || inputManager.verticalInput != 0)
        {
            // Complete the move mission
            MissionManager.Instance.CompleteMission("Move");
        }
    }


    //Rotating the player to face the direction of movement
    private void HandleRotation()
    {
        if (isJumping)
            return;

        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection = targetDirection + cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();

        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;


    }
    //Handles the falling and landing of the player
    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        Vector3 targetPosition;
        rayCastOrigin.y = rayCastOrigin.y + rayCastHeightOffSet;

        targetPosition = transform.position;


        if (!isGrounded && !isJumping)// If the player is not grounded and not jumping, then the player is falling
        {
            if (!playerManager.isInteracting)// If the player is not interacting with anything, then falling animation is played
            {
                animatorManager.PlayTargetAnimation("Falling", true);
            }

            animatorManager.animator.SetBool("isUsingRootMotion", false);// The player is not using root motion
            inAirTimer = inAirTimer + Time.deltaTime;

            playerRigidbody.AddForce(transform.forward * leapingVelocity);

            playerRigidbody.AddForce(-Vector3.up * fallingVelocity * inAirTimer);

            // Debugging: Indicate that the player is currently in the air and not grounded
           // Debug.Log("Player is in the air");
        }


        //Handliong the lighting and shadow of player
        if (Physics.SphereCast(rayCastOrigin, 0.2f, -Vector3.up, out hit, groundLayer))
        {
            if (!isGrounded && !playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Landing", true);

            }

            Vector3 rayCastHitPoint = hit.point;

            targetPosition.y = rayCastHitPoint.y;

            inAirTimer = 0;

            isGrounded = true;

            playerManager.isInteracting = false;

        }
        else
        {
            isGrounded = false;
        }

        if (isGrounded && !isJumping)
        {
            if (playerManager.isInteracting || inputManager.moveAmount > 0)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else
            {
                transform.position = targetPosition;
            }
        }

        //Debug.Log("IsInteracting boolean is set to " + playerManager.isInteracting); // To check if it true or false

    }

    public void HandleJumping()
    {
        if (isGrounded)
        {
            animatorManager.animator.SetBool("isJumping", true);
            animatorManager.PlayTargetAnimation("Jumping", false);

            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
            Vector3 playerVelocity = moveDirection;
            playerVelocity.y = jumpingVelocity;
            playerRigidbody.velocity = playerVelocity;

            // Complete the jump mission
            MissionManager.Instance.CompleteMission("Jump");
        }
    }


    public void HandleDodge()
    {
        if (playerManager.isInteracting)
            return;

        animatorManager.PlayTargetAnimation("Dodge", true, true);



    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something entered the trigger.");
        if (other.gameObject.CompareTag("Robot") && isPunching) // Ensure damage only occurs during an active punch
        {
            Debug.Log("Punch hit the robot.");
            RobotHealth robotHealth = other.GetComponent<RobotHealth>();
            if (robotHealth != null)
            {
                robotHealth.TakeDamage(10); // Assuming each punch deals 10 damage
            }
        }
    }


    private void HandleDash()
    {
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;

            if (dashTimer <= 0f)
            {
                isDashing = false;
                playerRigidbody.velocity = Vector3.zero;
            }
            else
            {
                playerRigidbody.velocity = dashDirection * dashDistance / dashDuration;
            }
        }
    }

    public void Dash()
    {
        if (!isDashing)
        {
            isDashing = true;
            dashTimer = dashDuration;
            dashDirection = cameraObject.forward * inputManager.verticalInput;
            dashDirection += cameraObject.right * inputManager.horizontalInput;
            dashDirection.Normalize();
            dashDirection.y = 0f;

            // Complete the dash mission
            MissionManager.Instance.CompleteMission("Dash");
        }
    }
}

