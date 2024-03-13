using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    PlayerMovement playerLocomotion;

    private void Awake()
    {
        inputManager = GetComponentInChildren<InputManager>();
        playerLocomotion = GetComponentInChildren<PlayerMovement>();
    }

    private void Update()
    {
        inputManager.HandleAllInputs();
    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovement();
    }
}
