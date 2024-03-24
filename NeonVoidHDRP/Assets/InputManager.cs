using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerMovement playerMovement;
    PickUpController pickUpController;
    AnimatorManager animatorManager;
    WeaponScript weaponScript;

    public Vector2 movementInput;
    public Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;

    public float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    public bool b_Input;
    public bool x_Input;
    public bool jump_Input;
    public bool shoot_Input;
    public bool pickUp_Input;

    public bool dash_Input;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();

        playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            playerControls.PlayerActions.B.performed += i => b_Input = true;
            playerControls.PlayerActions.B.canceled += i => b_Input = false;
            playerControls.PlayerActions.X.performed += i => x_Input = true;
            playerControls.PlayerActions.Jump.performed += i => jump_Input = true;
            playerControls.PlayerActions.Dash.performed += i => dash_Input = true;

            // Setup for Shoot and Pick Up actions
            playerControls.PlayerActions.Shoot.performed += _ => shoot_Input = true;
            playerControls.PlayerActions.Shoot.canceled += _ => shoot_Input = false;
            playerControls.PlayerActions.PickUp.performed += _ => pickUp_Input = true;
            playerControls.PlayerActions.PickUp.canceled += _ => pickUp_Input = false;
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprintingInput();
        HandleJumpingInput();
        HandleDodgeInput();
        HandleDashInput();
        HandleShootInput();
        HandlePickUpInput();
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputY = cameraInput.y;
        cameraInputX = cameraInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(0, moveAmount, playerMovement.isSprinting);

    }

    private void HandleSprintingInput()
    {
        if (b_Input && moveAmount > 0.5f)
        {
            playerMovement.isSprinting = true;

        }
        else
        {
            playerMovement.isSprinting = false;
        }
    }

    private void HandleJumpingInput()
    {
        if (jump_Input)
        {
            jump_Input = false;
            playerMovement.HandleJumping();
        }
    }
    private void HandleDashInput()
    {
        if (dash_Input)
        {
            dash_Input = false;
            playerMovement.Dash();
        }
    }

    private void HandleDodgeInput()
    {
        if (x_Input)
        {
            x_Input = false;
            playerMovement.HandleDodge();
        }
    }

    // Additional methods to handle Shoot and Pick Up inputs
    private void HandleShootInput()
    {
        if (shoot_Input)
        {
            shoot_Input = false; // Reset the input
                                 // Assuming you have a reference to the WeaponScript component
            weaponScript.Shoot();
        }
    }


    private void HandlePickUpInput()
    {
        if (pickUp_Input)
        {
            pickUpController.PickUpGun();
            pickUp_Input = false;  // Reset the pick up input
            
        }
    }

}
