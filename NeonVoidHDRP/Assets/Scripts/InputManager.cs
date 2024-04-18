using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerMovement playerMovement;
    PickUpController pickUpController;
    AnimatorManager animatorManager;

    public WeaponScript weaponScript;  // This will be assigned dynamically.

    public Vector2 movementInput;
    public Vector2 cameraInput;

    private CarController nearCar; // Car the player is near
    private bool isNearCar = false; // Is the player near the car

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
    public bool drop_Input;
    public bool car_exit_Input;


    public bool dash_Input;

    public bool isInCar = false;



    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        playerMovement = GetComponent<PlayerMovement>();
        pickUpController = GetComponent<PickUpController>();

        InitializeControls();
    }

    private void InitializeControls()
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
            playerControls.PlayerActions.Shoot.performed += _ => shoot_Input = true;
            playerControls.PlayerActions.Shoot.canceled += _ => shoot_Input = false;
            playerControls.PlayerActions.PickUp.performed += _ => pickUp_Input = true;
            playerControls.PlayerActions.PickUp.canceled += _ => pickUp_Input = false;
            playerControls.PlayerActions.Drop.performed += _ => drop_Input = true;
            playerControls.PlayerActions.Drop.canceled += _ => drop_Input = false;
            playerControls.PlayerActions.ExitCar.performed += _ => car_exit_Input = true;
            playerControls.PlayerActions.ExitCar.canceled += _ => car_exit_Input = false;

        }
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse button clicked directly checked");
        }
        HandleAllInputs();
    }

    public void HandleAllInputs()
    {
        if (isInCar)
        {
            HandleCarInput();
        }
        else
        {

            HandleStandardInputs();          
        }

        if (car_exit_Input && isInCar)
        {
            HandleExitCar();
        }
    }

    private void HandleStandardInputs()
    {
        HandleMovementInput();
        HandleSprintingInput();
        HandleJumpingInput();
        HandleDodgeInput();
        HandleDashInput();
        HandleShootInput();
        HandlePickUpInput();
        HandleDropInput();
    }

    private void HandleCarInput()
    {
        // Handle car-specific inputs like driving, braking, etc.
    }

    private void HandleExitCar()
    {
        Debug.Log("Exiting the car now.");
        //nearCar.ExitCar(this);
        isInCar = false;
        car_exit_Input = false; // Ensure to reset to avoid repeating exit
    }

    // Add similar method for entering the car
    public void EnterCar()
    {
        Debug.Log("Entering the car now.");
        isInCar = true;
        // Code to switch controls to the car and adjust the camera
    }

    public void SetNearCar(CarController car, bool isNear)
    {
        nearCar = isNear ? car : null;
        isNearCar = isNear;
    }



    private void HandleDropInput()
    {
        if (drop_Input)
        {
            Debug.Log("Drop input received.");
            drop_Input = false; // Reset the input flag
            if (pickUpController != null)
            {
                pickUpController.DropGun(); // Call DropGun method
            }
            else
            {
                Debug.LogError("PickUpController is not assigned.");
            }
        }
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
        playerMovement.isSprinting = (b_Input && moveAmount > 0.5f);
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

    private void HandleShootInput()
    {
        if (shoot_Input && weaponScript != null && weaponScript.canShoot)
        {
            Debug.Log("Shooting input received and weapon is ready.");
            shoot_Input = false;
            weaponScript.Shoot();
        }
        else
        {
            if (shoot_Input)
            {
                Debug.Log("Shoot input received but weapon is not ready or not assigned.");
            }
        }
    }

    private void HandlePickUpInput()
    {
        if (pickUp_Input)
        {
            if (isNearCar && nearCar != null)
            {
                // Enter the car and pass this InputManager instance
                //nearCar.EnterCar(this);
                // Note: The camera switch is handled within the EnterCar() now
                isInCar = true;
            }
            else
            {
                // Proceed to pick up items as usual if not near the car
                pickUpController.PickUpGun();
            }
            // Reset the pickUp_Input flag after processing
            pickUp_Input = false;
        }
    }


    // Dynamic assignment of weaponScript
    public void AssignWeaponScript(WeaponScript newWeaponScript)
    {
        weaponScript = newWeaponScript;
        weaponScript.canShoot = true;
        Debug.Log("WeaponScript assigned and can shoot enabled");
    }

}
