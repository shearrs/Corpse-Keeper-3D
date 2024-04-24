using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : PlayerTool
{
    [SerializeField] private GameObject gfx;
    [SerializeField] private Light flashlight;
    private Animator playerAnimator;

    private readonly int flashlightID = Animator.StringToHash("flashlight");

    private Animator PlayerAnimator
    {
        get
        {
            if (playerAnimator == null)
                playerAnimator = PlayerToolManager.PlayerAnimation.Animator;

            return playerAnimator;
        }
    }

    private void Update()
    {
        if (flashlight.gameObject.activeSelf)
        {
            PlayerToolManager.PlayerInteraction.CheckInteraction();
        }
    }

    public override void Disable()
    {
        PlayerAnimator.SetBool(flashlightID, false);

        StopAllCoroutines();
        StartCoroutine(IEDelayActivation(false));
    }

    public override void Enable()
    {
        PlayerAnimator.SetBool(flashlightID, true);
        gfx.SetActive(false);

        StopAllCoroutines();
        StartCoroutine(IEDelayActivation(true));
    }

    public override void Use()
    {
        flashlight.gameObject.SetActive(!flashlight.gameObject.activeSelf);
    }

    private IEnumerator IEDelayActivation(bool enable)
    {
        while (PlayerAnimator.IsInTransition(0))
            yield return null;

        while (!PlayerAnimator.IsPlaying("Idle") && !PlayerAnimator.IsPlaying("FlashlightHold"))
            yield return null;

        gfx.SetActive(enable);
        flashlight.gameObject.SetActive(enable);
    }
}
