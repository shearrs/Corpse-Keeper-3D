using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hands : PlayerTool
{
    [SerializeField] private GameObject fly;
    private Animator animator;

    private readonly int handsID = Animator.StringToHash("hands");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (PlayerToolManager.CurrentTool is Hands)
        {
            if (PlayerToolManager.IsHolding && !fly.activeSelf)
                fly.SetActive(true);
            else if (!PlayerToolManager.IsHolding && fly.activeSelf)
                fly.SetActive(false);
        }
        else if (fly.activeSelf)
            fly.SetActive(false);
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
        PlayerToolManager.PlayerInteraction.CheckInteraction();

        if (PlayerToolManager.IsHolding)
            return;

        IsBeingUsed = true;

        Animator animator = PlayerToolManager.PlayerAnimation.Animator;

        if (!PlayerToolManager.IsHolding)
            animator.PlayAndNotify(this, "Grab", () => IsBeingUsed = false);
    }
}
