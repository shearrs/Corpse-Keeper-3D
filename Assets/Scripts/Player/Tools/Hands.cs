using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hands : PlayerTool
{
    public override void Enable()
    {
    }

    public override void Disable()
    {
    }

    public override void Use()
    {
        IsBeingUsed = true;
        PlayerToolManager.PlayerInteraction.CheckInteraction();

        Animator animator = PlayerToolManager.PlayerAnimation.Animator;
        animator.PlayAndNotify(this, "Grab", () => IsBeingUsed = false);
    }
}
