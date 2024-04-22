using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerControl;

public class Player : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private PlayerStats stats;
    [SerializeField] private Animator animator;
    private CustomController controller;
    private PlayerCamera playerCamera;
    private PlayerAnimation playerAnimation;

    [Header("State Machine")]
    [SerializeField] private PlayerStateMachine stateMachine;

    public PlayerStats Stats => stats;
    public PlayerFlags Flags { get; private set; }
    public PlayerMovement PlayerMovement { get; private set; }

    private void Start()
    {
        controller = GetComponent<CustomController>();
        playerCamera = Camera.main.GetComponent<PlayerCamera>();

        PlayerMovement = new(controller);
        Flags = new(controller);
        stateMachine = new(this);
        playerAnimation = new(controller, animator);

        PlayerToolManager.PlayerAnimation = playerAnimation;

        PlayerInputHandler.JumpBuffer = Stats.JumpBuffer;
        PlayerInputHandler.RelativeMovementTransform = transform;
        PlayerInputHandler.Enable();
    }

    private void Update()
    {
        stateMachine.Update();
        PlayerMovement.Update();
        playerAnimation.Update();

        RotateTowardsCamera();
    }

    private void RotateTowardsCamera()
    {
        Quaternion targetRotation = playerCamera.transform.rotation;
        targetRotation.x = 0;
        targetRotation.z = 0;

        transform.rotation = targetRotation;
    }
}
