using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "New Button Data", menuName = "Button Data")]
    public class ButtonData : ScriptableObject
    {
        [SerializeField] private Color enabledColor;
        [SerializeField] private Color disabledColor;
        [SerializeField] private Color highlightedColor;
        [SerializeField] private Color pressedColor;

        public Color EnabledColor => enabledColor;
        public Color DisabledColor => disabledColor;
        public Color HighlightedColor => highlightedColor;
        public Color PressedColor => pressedColor;
    }
}