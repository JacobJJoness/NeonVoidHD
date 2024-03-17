using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    CameraManager cameraManager;
    Animator animator;
    PlayerMovement playerMovement;

    public bool isInteracting;

    private void Awake()
    {
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

        isInteracting = animator.GetBool("isInteracting");

        playerMovement.isJumping = animator.GetBool("isJumping");

        animator.SetBool("isGrounded", playerMovement.isGrounded);
    }
}
