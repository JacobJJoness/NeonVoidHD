using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerMovement playerMovement;
    PickUpController pickUpController;
    AnimatorManager animatorManager;
    CameraManager cameraManager;

    public WeaponScript weaponScript;  // Dynamically assigned.

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
    public bool drop_Input;
    public bool car_exit_Input;

    public bool dash_Input;
    public bool isInCar; // Flag to check if the player is inside the car
    public GameObject playerModel; // Assign the player model in the inspector

    public bool isNearCar; // Flag to check if the player is near a car
    public CarController nearCar; // Reference to the nearby car

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        playerMovement = GetComponent<PlayerMovement>();
        pickUpController = GetComponent<PickUpController>();
        cameraManager = GetComponent<CameraManager>();

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
            playerControls.PlayerActions.X.canceled += i => x_Input = false;
            playerControls.PlayerActions.Jump.performed += i => jump_Input = true;
            playerControls.PlayerActions.Jump.canceled += i => jump_Input = false;
            playerControls.PlayerActions.Dash.performed += i => dash_Input = true;
            playerControls.PlayerActions.Dash.canceled += i => dash_Input = false;
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
        HandleMovementInput();
        HandleSprintingInput();
        HandleJumpingInput();
        HandleDodgeInput();
        HandleDashInput();
        HandleShootInput();
        HandlePickUpInput();
        HandleDropInput();
        HandleCarInputs();
    }

    private void HandleCarInputs()
    {
        if (car_exit_Input && isInCar)
        {
            HandleExitCar();
        }
        else if (car_exit_Input && isNearCar && !isInCar)
        {
            EnterCar(nearCar);
        }
        car_exit_Input = false; // Reset the car exit input flag
    }

    private void HandleDropInput()
    {
        if (drop_Input)
        {
            Debug.Log("Drop input received.");
            drop_Input = false;
            if (pickUpController != null)
            {
                pickUpController.DropGun();
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
            pickUpController.PickUpGun();
            pickUp_Input = false;
        }
    }

    public void AssignWeaponScript(WeaponScript newWeaponScript)
    {
        weaponScript = newWeaponScript;
        weaponScript.canShoot = true;
        Debug.Log("WeaponScript assigned and can shoot enabled");
    }

    public void HandleExitCar()
    {
        if (!isInCar) return;

        isInCar = false;
        playerModel.SetActive(true);
        Debug.Log("Exited the car.");
    }

    public void EnterCar(CarController car)
    {
        if (!isNearCar || nearCar == null) return;

        isInCar = true;
        playerModel.SetActive(false);
        cameraManager.ChangeTarget(car.carCameraTransform);
        Debug.Log("Entered the car.");
    }

    public void SetNearCar(CarController car, bool isNear)
    {
        nearCar = isNear ? car : null;
        isNearCar = isNear;

        // Add debug statements to log the status change
        if (isNear)
        {
            Debug.Log("Player is now near the car: " + car.gameObject.name);
        }
        else
        {
            Debug.Log("Player is no longer near the car");
        }
    }
}
