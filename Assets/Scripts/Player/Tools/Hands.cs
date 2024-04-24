using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hands : PlayerTool
{
    private Animator animator;

    private readonly int handsID = Animator.StringToHash("hands");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void Enable()
    {
        animator.SetBool(handsID, true);
    }

    public override void Disable()
    {
        animator.SetBool(handsID, false);
    }

    public override void Use()
    {
        IsBeingUsed = true;
        PlayerToolManager.PlayerInteraction.CheckInteraction();

        Animator animator = PlayerToolManager.PlayerAnimation.Animator;

        if (!PlayerToolManager.IsHolding)
            animator.PlayAndNotify(this, "Grab", () => IsBeingUsed = false);
    }
}
