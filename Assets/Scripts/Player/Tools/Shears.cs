using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shears : PlayerTool
{
    private Animator animator;

    private readonly int cutID = Animator.StringToHash("Cut");
    private readonly int shearsCutID = Animator.StringToHash("ShearsCut");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void Disable()
    {
        gameObject.SetActive(false);
    }

    public override void Enable()
    {
        gameObject.SetActive(true);
    }

    public override void Use()
    {
        IsBeingUsed = true;
        PlayerToolManager.PlayerInteraction.CheckInteraction();

        StartCoroutine(IECut());
    }

    private IEnumerator IECut()
    {
        Animator playerAnimator = PlayerToolManager.PlayerAnimation.Animator;

        playerAnimator.Play(shearsCutID);
        animator.Play(cutID);

        while (playerAnimator.IsPlaying("ShearsCut") || playerAnimator.IsInTransition(0))
            yield return null;

        while (animator.IsPlaying("Cut") || animator.IsInTransition(0))
            yield return null;

        IsBeingUsed = false;
    }
}
