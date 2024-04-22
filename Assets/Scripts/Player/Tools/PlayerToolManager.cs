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
    private PlayerTool currentTool;

    public static PlayerTool CurrentTool { get => Instance.currentTool; set => Instance.SetTool(value); }

    private void Start()
    {
        CurrentTool = shears;
    }

    private void SetTool(PlayerTool tool)
    {
        if (tool == CurrentTool)
            return;

        CurrentTool.Disable();

        currentTool = tool;

        CurrentTool.Enable();
    }

    private void Update()
    {
        if (CurrentTool.IsBeingUsed && PlayerInputHandler.InteractInput)
            interaction.CheckInteraction();
    }

    private void OnDrawGizmosSelected()
    {
        interaction.DrawGizmos();
    }
}