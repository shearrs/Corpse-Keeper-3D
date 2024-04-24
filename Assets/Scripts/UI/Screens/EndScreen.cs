using System.Collections;
using System.Collections.Generic;
using TMPro;
using Tweens;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EndScreen : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private MenuButton menuButton;
        private Color backgroundColor;
        private Color transparentColor;

        private void Awake()
        {
            backgroundColor = background.color;
            transparentColor = backgroundColor;
            transparentColor.a = 0;
        }

        public void Enable()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            PlayerInputHandler.Disable();

            background.color = transparentColor;
            TweenBackground();
        }

        private void TweenBackground()
        {
            background.gameObject.SetActive(true);

            void update(float percentage)
            {
                background.color = Color.Lerp(transparentColor, backgroundColor, percentage);
            }

            TweenManager.DoTweenCustom(update, 2f).SetOnComplete(EnableElements);
        }

        private void EnableElements()
        {
            title.gameObject.SetActive(true);
            menuButton.gameObject.SetActive(true);
        }
    }
}