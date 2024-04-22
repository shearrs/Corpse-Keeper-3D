using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;
using Tweens;

public class StartButton : Button3D
{
    [SerializeField] private Vector3 movePosition;

    private bool isMoved;
    private Vector3 startPosition;
    private Tween moveTween;

    protected override void Awake()
    {
        base.Awake();

        startPosition = transform.position;
    }

    private void Update()
    {
        if (isPressed && isHovered && !isMoved)
        {
            moveTween?.Stop();

            moveTween = transform.DoTweenPosition(movePosition, 0.2f);
            isMoved = true;
        }
        else if ((!isPressed || !isHovered) && isMoved)
        {
            moveTween?.Stop();

            moveTween = transform.DoTweenPosition(startPosition, 0.2f);
            isMoved = false;
        }
    }

    public override void OnClicked()
    {
        Disable();

        Loader.LoadUnLoad("Game", "MainMenu");
    }
}