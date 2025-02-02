using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : Singleton<PlayerInputHandler>
{
    private static PlayerInputActions InputActions => Instance.inputActions;
    private static PlayerInputActions.PlayerActions PlayerInput => InputActions.Player;

    // inputs
    public static Vector3 MovementInput
    {
        get
        {
            if (RelativeMovementTransform == null)
                return Vector3.zero;

            return Instance.CalculateMovement();
        }
    }
    public static Vector2 CameraMovementInput => PlayerInput.CameraMovement.ReadValue<Vector2>();
    public static bool InteractInput => PlayerInput.Interact.WasPressedThisFrame();
    public static bool SprintInput => PlayerInput.Sprint.IsPressed();
    public static bool JumpInput
    {
        get
        {
            if (JumpBuffer == null)
                return false;

            return !JumpBuffer.Done;
        }
    }
    public static bool Tool1Input => PlayerInput.Tool1.WasPressedThisFrame();
    public static bool Tool2Input => PlayerInput.Tool2.WasPressedThisFrame();
    public static bool Tool3Input => PlayerInput.Tool3.WasPressedThisFrame();

    // references
    public static Cooldown JumpBuffer { get => Instance.jumpBuffer; set => Instance.jumpBuffer = value; }
    public static Transform RelativeMovementTransform { get => Instance.relativeMovementTransform; set => Instance.relativeMovementTransform = value; }

    private Cooldown jumpBuffer;
    private Transform relativeMovementTransform;
    private PlayerInputActions inputActions;

    protected override void Awake()
    {
        base.Awake();

        inputActions = new();
    }

    public static void Enable()
    {
        InputActions.Enable();
        PlayerInput.Jump.performed += Instance.StartJumpBuffer;
    }

    public static void Disable()
    {
        Instance.inputActions.Disable();
        PlayerInput.Jump.performed -= Instance.StartJumpBuffer;
    }

    private void StartJumpBuffer(InputAction.CallbackContext ctx)
    {
        jumpBuffer.RestartTimer();
    }

    private Vector3 CalculateMovement()
    {
        Vector2 movementInput = PlayerInput.Movement.ReadValue<Vector2>();
        Vector3 movement;

        Vector3 forward = relativeMovementTransform.forward;
        forward.y = 0;
        forward.Normalize();

        movement = relativeMovementTransform.forward * movementInput.y;
        movement += relativeMovementTransform.right * movementInput.x;

        return movement.normalized;
    }
}
