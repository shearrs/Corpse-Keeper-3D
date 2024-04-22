using UnityEngine;
using Tweens;
using UI;

public abstract class Button3D : MonoBehaviour
{
    private const float TWEEN_TIME = 0.1f;

    [SerializeField] private ButtonData data;

    private Material material;
    private Tween colorTween;
    protected bool isEnabled;
    protected bool isHovered;
    protected bool isPressed;

    protected virtual void Awake()
    {
        isEnabled = true;
        material = GetComponent<MeshRenderer>().material;
    }

    public virtual void Enable()
    {
        material.color = data.EnabledColor;
        isEnabled = true;
    }

    public virtual void Disable()
    {
        material.color = data.DisabledColor;
        isEnabled = false;
    }

    public abstract void OnClicked();

    private void TweenColor(Color target)
    {
        colorTween?.Stop();

        Color start = material.color;
        void update(float percentage)
        {
            material.color = Color.Lerp(start, target, percentage);
        }

        colorTween = TweenManager.DoTweenCustom(update, TWEEN_TIME);
    }

    private void OnMouseEnter()
    {
        if (!isEnabled)
            return;

        isHovered = true;

        if (isPressed)
            TweenColor(data.PressedColor);
        else
            TweenColor(data.HighlightedColor);
    }

    private void OnMouseExit()
    {
        isHovered = false;
        TweenColor(data.EnabledColor);
    }

    private void OnMouseDown()
    {
        if (!isEnabled)
            return;

        isPressed = true;
        TweenColor(data.PressedColor);
    }

    private void OnMouseUp()
    {
        isPressed = false;

        if (isHovered)
        {
            TweenColor(data.HighlightedColor);
            OnClicked();
        }
        else
            TweenColor(data.EnabledColor);
    }
}
