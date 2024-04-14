using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tweens
{
    public static class TweenManager
    {
        public static Vector3 TWEEN_ZERO = Vector3.one * 0.001f;

        private static readonly List<Tween> availableTweens = new();
        private static readonly List<Tween> activeTweens = new();

        #region Transforms

        #region Positions
        public static Tween DoTweenPosition(this Transform transform, Vector3 to, float duration, bool clamp = true)
        {
            Tween tween = CreateTweenPosition(transform, to, duration, clamp);

            tween.Start();

            return tween;
        }

        public static Tween CreateTweenPosition(this Transform transform, Vector3 to, float duration, bool clamp = true)
        {
            Tween tween = GetTween();

            Vector3 from = transform.position;
            void update(float percentage) => transform.position = Vector3.LerpUnclamped(from, to, percentage);

            UpdateTweenData(tween, duration, update);

            if (clamp)
                tween.SetOnComplete(() => transform.position = to);

            return tween;
        }

        public static Tween DoTweenPosition(this RectTransform transform, Vector3 to, float duration, bool clamp = true)
        {
            Tween tween = GetTween();

            Vector3 from = transform.anchoredPosition3D;
            void update(float percentage) => transform.anchoredPosition3D = Vector3.LerpUnclamped(from, to, percentage);

            UpdateTweenData(tween, duration, update);

            if (clamp)
                tween.SetOnComplete(() => transform.anchoredPosition3D = to);

            tween.Start();

            return tween;
        }
        #endregion

        #region Rotations
        public static Tween DoTweenRotation(this Transform transform, Quaternion to, float duration, bool clamp = true)
        {
            Tween tween = CreateTweenRotation(transform, to, duration, clamp);
            tween.Start();

            return tween;
        }

        public static Tween CreateTweenRotation(this Transform transform, Quaternion to, float duration, bool clamp = true)
        {
            Tween tween = GetTween();
            Quaternion from = transform.localRotation;
            void update(float percentage) => transform.localRotation = Quaternion.LerpUnclamped(from, to, percentage);

            UpdateTweenData(tween, duration, update);

            if (clamp)
                tween.SetOnComplete(() => transform.localRotation = to);

            return tween;
        }
        #endregion

        #region Scales

        public static Tween DoTweenScale(this Transform transform, Vector3 to, float duration, bool clamp = true)
        {
            Tween tween = CreateTweenScale(transform, to, duration, clamp);
            tween.Start();

            return tween;
        }

        public static Tween CreateTweenScale(this Transform transform, Vector3 to, float duration, bool clamp = true)
        {
            Tween tween = GetTween();

            Vector3 from = transform.localScale;
            void update(float percentage) => transform.localScale = Vector3.LerpUnclamped(from, to, percentage);

            UpdateTweenData(tween, duration, update);

            if (clamp)
                tween.SetOnComplete(() => transform.localScale = to);

            return tween;
        }

        #endregion

        #endregion

        #region Color
        public static Tween DoTweenColor(this Image image, Color to, float duration, bool clamp = true)
        {
            Tween tween = CreateTweenColor(image, to, duration, clamp);
            tween.Start();

            return tween;
        }

        public static Tween CreateTweenColor(this Image image, Color to, float duration, bool clamp = true)
        {
            Tween tween = GetTween();

            Color from = image.color;
            void update(float percentage) => image.color = Color.LerpUnclamped(from, to, percentage);

            UpdateTweenData(tween, duration, update);

            if (clamp)
                tween.SetOnComplete(() => image.color = to);

            return tween;
        }
        #endregion

        #region Custom
        public static Tween DoTweenCustom(Action<float> update, float duration)
        {
            Tween tween = CreateTweenCustom(update, duration);

            tween.Start();

            return tween;
        }

        public static Tween CreateTweenCustom(Action<float> update, float duration)
        {
            Tween tween = GetTween();

            UpdateTweenData(tween, duration, update);

            return tween;
        }

        #endregion

        public static Tween Shake(this Transform transform, float amount, float duration)
        {
            Tween tween = GetTween();

            Vector3 originalPosition = transform.position;
            void update(float percentage)
            {
                float magnitude = (1 - percentage) * amount;
                transform.position = originalPosition + (Vector3)UnityEngine.Random.insideUnitCircle * magnitude;
            }

            UpdateTweenData(tween, duration, update);
            tween.SetOnComplete(() => transform.position = originalPosition);
            tween.Start();

            return tween;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void ClearTweens()
        {
            availableTweens.Clear();
            activeTweens.Clear();
        }

        private static void UpdateTweenData(Tween tween, float duration, Action<float> update)
        {
            tween.Pause();
            tween.SetDuration(duration);
            tween.SetOnComplete(null);
            tween.SetEasingFunction(EasingFunctions.EasingFunction.LINEAR);
            tween.SetUpdate(update);
        }

        private static Tween GetTween()
        {
            Tween tween;

            if (availableTweens.Count == 0)
            {
                tween = new();
            }
            else
            {
                tween = availableTweens[0];
                availableTweens.RemoveAt(0);
            }

            activeTweens.Add(tween);

            return tween;
        }

        public static void StopIfPlaying(Tween tween)
        {
            if (tween != null && tween.IsPlaying)
                tween.Stop();
        }
        public static void ReturnTween(Tween tween)
        {
            if(activeTweens.Remove(tween))
                availableTweens.Add(tween);
        }
    }
}