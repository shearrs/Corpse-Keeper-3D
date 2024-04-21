using System;
using Tweens;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public abstract class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        private const float TWEEN_TIME = 0.1f;

        [SerializeField] private ButtonData data;

        private Image image;
        private Tween tween;
        private bool isEnabled;
        private bool isHovered;
        private bool isPressed;

        protected void Awake()
        {
            image = GetComponent<Image>();

            isEnabled = true;
        }

        public virtual void Enable()
        {
            image.color = data.EnabledColor;
            isEnabled = true;
        }

        public virtual void Disable()
        {
            image.color = data.DisabledColor;
            isEnabled = false;
        }

        public abstract void OnClicked();

        private void TweenColor(Color target)
        {
            tween?.Stop();

            Color start = image.color;
            void update(float percentage)
            {
                image.color = Color.Lerp(start, target, percentage);
            }

            tween = TweenManager.DoTweenCustom(update, TWEEN_TIME);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!isEnabled)
                return;

            isHovered = true;

            if (isPressed)
                TweenColor(data.PressedColor);
            else
                TweenColor(data.HighlightedColor);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!isEnabled)
                return;

            isHovered = false;
            TweenColor(data.EnabledColor);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!isEnabled)
                return;

            isPressed = true;
            TweenColor(data.PressedColor);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!isEnabled)
                return;

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
}