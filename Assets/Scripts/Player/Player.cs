using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerControl;

public class Player : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private PlayerStats stats;
    [SerializeField] private PlayerInteraction interaction;
    private CustomController controller;
    private PlayerCamera playerCamera;

    [Header("State Machine")]
    [SerializeField] private PlayerStateMachine stateMachine;

    public PlayerStats Stats => stats;
    public PlayerFlags Flags { get; private set; }
    public PlayerMovement PlayerMovement { get; private set; }

    private void Start()
    {
        controller = GetComponent<CustomController>();
        PlayerMovement = new(controller);
        Flags = new(controller);
        stateMachine = new(this);
        playerCamera = Camera.main.GetComponent<PlayerCamera>();

        PlayerInputHandler.JumpBuffer = Stats.JumpBuffer;
        PlayerInputHandler.RelativeMovementTransform = playerCamera.transform;
        PlayerInputHandler.Enable();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        interaction.Update();
        stateMachine.Update();
        PlayerMovement.Update();

        RotateTowardsCamera();
    }

    private void RotateTowardsCamera()
    {
        Quaternion targetRotation = playerCamera.transform.rotation;
        targetRotation.x = 0;
        targetRotation.z = 0;

        transform.rotation = targetRotation;
    }

    private void OnDrawGizmosSelected()
    {
        interaction.DrawGizmos(); 
    }
}
