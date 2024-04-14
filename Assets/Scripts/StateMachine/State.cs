using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomStateMachine
{
    [System.Serializable]
    public abstract class State
    {
        private State subState;
        protected State defaultSubState;

        public Transition[] Transitions { get; set; }
        public State SuperState { get; set; }
        public State SubState
        {
            get => subState;
            set
            {
                if (subState != null)
                {
                    subState.SuperState = null;
                    subState.OnExit();
                }

                subState = value;

                if (subState != null)
                {
                    subState.SuperState = this;
                    subState.OnEnter();
                }
            }
        }
        
        public void OnEnter()
        {
            SubState = InitializeSubState();
            OnEnterInternal();
        }
        protected abstract void OnEnterInternal();

        protected virtual State InitializeSubState()
        {
            if (defaultSubState == null)
                return null;

            if (defaultSubState.UniversalTransition())
                return defaultSubState;
            else
            {
                State newState = defaultSubState.CheckTransitions();

                if (newState != null)
                    return newState;

                return defaultSubState;
            }
        }

        public void OnExit()
        {
            SubState = null;
            OnExitInternal();
        }

        protected abstract void OnExitInternal();

        public void Update()
        {
            UpdateSubstate();

            UpdateInternal();
        }
        protected abstract void UpdateInternal();

        public virtual bool UniversalTransition()
        {
            return true;
        }

        public virtual State CheckTransitions()
        {
            if (Transitions == null)
                return null;

            foreach (Transition transition in Transitions)
            {
                if (transition.Condition())
                    return transition.To;
            }

            return null;
        }

        private void UpdateSubstate()
        {
            if (SubState == null)
                return;

            State newState = SubState.CheckTransitions();
            if (newState != null)
                SubState = newState;

            SubState.Update();
        }
    }
}