using PlayerControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToolManager : Singleton<PlayerToolManager>
{
    [SerializeField] private Hands hands;
    [SerializeField] private Shears shears;
    [SerializeField] private Flashlight flashlight;
    [SerializeField] private PlayerInteraction interaction;
    private PlayerAnimation playerAnimation;
    private PlayerTool currentTool;
    private bool isHolding;

    public static PlayerInteraction PlayerInteraction => Instance.interaction;
    public static PlayerAnimation PlayerAnimation { get => Instance.playerAnimation; set => Instance.playerAnimation = value; }
    public static PlayerTool CurrentTool { get => Instance.currentTool; set => Instance.SetTool(value); }
    public static bool IsHolding { get => Instance.isHolding; set => Instance.isHolding = value; }

    private void Start()
    {
        CurrentTool = hands;
    }

    private void Update()
    {
        CheckInput();

        if (!CurrentTool.IsBeingUsed && PlayerInputHandler.InteractInput)
            CurrentTool.Use();
    }

    private void CheckInput()
    {
        if (PlayerInputHandler.Tool1Input)
            CurrentTool = hands;
        else if (PlayerInputHandler.Tool2Input)
            CurrentTool = shears;
        else if (PlayerInputHandler.Tool3Input)
            CurrentTool = flashlight;
    }

    private void SetTool(PlayerTool tool)
    {
        if (tool == CurrentTool)
            return;

        if (CurrentTool != null)
        {
            if (CurrentTool.IsBeingUsed)
                return;

            CurrentTool.Disable();
        }

        currentTool = tool;

        CurrentTool.Enable();
    }

    private void OnDrawGizmosSelected()
    {
        interaction.DrawGizmos();
    }
}