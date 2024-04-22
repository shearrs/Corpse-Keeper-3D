using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweens;

public class TitleCard : MonoBehaviour
{
    [SerializeField] private Vector3 topHoverPosition;
    private Tween tween;

    private void OnDisable()
    {
        tween?.Stop();
    }

    private void Start()
    {
        tween = transform.DoTweenPosition(topHoverPosition, 1f, false).SetEasingFunction(EasingFunctions.EasingFunction.OUT_BACK).SetLooping(Tween.LoopType.PING_PONG);
    }
}
