using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shears : PlayerTool
{
    [SerializeField] private GameObject gfx;
    private Animator animator;
    private Animator playerAnimator;

    private readonly int shearsID = Animator.StringToHash("shears");
    private readonly int cutID = Animator.StringToHash("Cut");
    private readonly int shearsCutID = Animator.StringToHash("ShearsCut");

    private Animator PlayerAnimator
    {
        get
        {
            if (playerAnimator == null)
                playerAnimator = PlayerToolManager.PlayerAnimation.Animator;

            return playerAnimator;
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void Disable()
    {
        PlayerAnimator.SetBool(shearsID, false);

        StopAllCoroutines();
        StartCoroutine(IEDelayActivation(false));
    }

    public override void Enable()
    {
        PlayerAnimator.SetBool(shearsID, true);
        gfx.SetActive(false);

        StopAllCoroutines();
        StartCoroutine(IEDelayActivation(true));
    }

    public override void Use()
    {
        IsBeingUsed = true;
        PlayerToolManager.PlayerInteraction.CheckInteraction();

        StartCoroutine(IECut());
    }

    private IEnumerator IEDelayActivation(bool enable)
    {
        while (PlayerAnimator.IsInTransition(0))
            yield return null;

        while (!PlayerAnimator.IsPlaying("Idle"))
            yield return null;
            
        gfx.SetActive(enable);
    }

    private IEnumerator IECut()
    {
        PlayerAnimator.Play(shearsCutID);
        animator.Play(cutID);

        while (PlayerAnimator.IsPlaying("ShearsCut") || PlayerAnimator.IsInTransition(0))
            yield return null;

        while (animator.IsPlaying("Cut") || animator.IsInTransition(0))
            yield return null;

        IsBeingUsed = false;
    }
}
