using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Tweens
{
    [Serializable]
    public class Tween
    {
        public enum LoopType { NONE, PING_PONG, REPEAT }
        public const int INFINITE_LOOPS = -1;

        [Header("General")]
        [SerializeField] private float duration = 1f;

        [Header("Looping")]
        [SerializeField] private LoopType loopType = LoopType.NONE;
        [SerializeField] private int loops = 0;

        [Header("Easing")]
        [SerializeField] private EasingFunctions.EasingFunction easingFunction = EasingFunctions.EasingFunction.LINEAR;
        [SerializeField] private bool useCustomCurve = false;
        [SerializeField] private AnimationCurve curve;

        private float progress = 0;
        private CancellationTokenSource tokenSource;
        private Action<float> Update;
        private readonly List<Action> OnCompletes = new();

        #region Accessors
        public EasingFunctions.EasingFunction EasingFunction => easingFunction;
        public float Duration { get => duration; private set => duration = value; }
        public float TimePassed { get; private set; } = 0;
        public float Percentage
        {
            get => progress / Duration;
            set => progress = value * Duration;
        }
        public bool IsPlaying { get; private set; } = false;
        private bool IsLooping => loops > 0 || loops == INFINITE_LOOPS;
        #endregion

        #region Constructors
        public Tween()
        {
            Duration = 0;
            easingFunction = EasingFunctions.EasingFunction.LINEAR;
        }

        public Tween(Action<float> update, float duration)
        {
            Debug.Log("constructed here");

            Duration = duration;
            Update = update;
            easingFunction = EasingFunctions.EasingFunction.LINEAR;
        }

        ~Tween()
        {
            Stop();
            tokenSource?.Dispose();
        }
        #endregion

        #region Setters
        public Tween SetDuration(float duration)
        {
            Duration = duration;

            return this;
        }

        public Tween SetUpdate(Action<float> update)
        {
            Update = update;

            return this;
        }

        public Tween SetOnComplete(Action onComplete)
        {
            if (onComplete == null)
                OnCompletes.Clear();
            else
                OnCompletes.Add(onComplete);

            return this;
        }

        public Tween SetLooping(LoopType type = LoopType.REPEAT, int loops = INFINITE_LOOPS)
        {
            this.loops = loops;
            loopType = type;

            return this;
        }

        public Tween SetEasingFunction(EasingFunctions.EasingFunction easingFunction)
        {
            this.easingFunction = easingFunction;

            return this;
        }
        #endregion

        public void Start()
        {
            if (Duration == 0)
            {
                Update?.Invoke(1);
                return;
            }

            if (!IsPlaying)
            {
                tokenSource = new CancellationTokenSource();

                TimePassed = 0;
                progress = 0;

                PlayAsync(tokenSource.Token);
            }
        }

        public void Pause()
        {
            if (tokenSource != null)
            {
                tokenSource.Cancel();
                tokenSource.Dispose();
                tokenSource = null;
            }

            IsPlaying = false;
        }

        public void Stop()
        {
            if (!IsPlaying)
                return;

            Pause();

            PlayOnCompletes();
        }

        private async void PlayAsync(CancellationToken token)
        {
            IsPlaying = true;

            while (Percentage < 1.0)
            {
                if (token.IsCancellationRequested)
                {
                    TweenManager.ReturnTween(this);
                    return;
                }
                else if (!Application.isPlaying)
                    return;

                TimePassed += Time.deltaTime;
                progress += Time.deltaTime;

                UpdateValue();
                await Task.Yield();
            }

            progress = Duration;

            if (IsLooping)
                Loop(token);
            else
            {
                Stop();
                TweenManager.ReturnTween(this);
            }
        }

        private void UpdateValue(bool loop = false)
        {
            float easedPercentage;

            if (useCustomCurve)
                easedPercentage = curve.Evaluate(Percentage);
            else if (loop)
                easedPercentage = 1 - EasingFunctions.EasePercentage(easingFunction, 1 - Percentage);
            else
                easedPercentage = EasingFunctions.EasePercentage(easingFunction, Percentage);

            Update?.Invoke(easedPercentage);
        }

        private void PlayOnCompletes()
        {
            for (int i = 0; i < OnCompletes.Count; i++)
            {
                OnCompletes[i]?.Invoke();
            }
        }

        #region Looping
        private void Loop(CancellationToken token)
        {
            switch(loopType)
            {
                case LoopType.NONE:
                    break;
                case LoopType.REPEAT:
                    LoopRepeatAsync(token);
                    break;
                case LoopType.PING_PONG:
                    LoopPingPongAsync(token);
                    break;
            }
        }

        private async void LoopPingPongAsync(CancellationToken token)
        {
            while (Percentage > 0.0)
            {
                if (token.IsCancellationRequested || !Application.isPlaying)
                    return;

                TimePassed += Time.deltaTime;
                progress -= Time.deltaTime;

                UpdateValue(true);
                await Task.Yield();
            }

            DecrementLoops();

            if (IsLooping && token != null)
                PlayAsync(token);
            else
                Stop();
        }

        private void LoopRepeatAsync(CancellationToken token)
        {
            DecrementLoops();

            if (IsLooping)
            {
                progress = 0;
                PlayAsync(token);
            }
        }

        private void DecrementLoops()
        {
            if (loops > 0)
                loops--;
        }
        #endregion
    }
}