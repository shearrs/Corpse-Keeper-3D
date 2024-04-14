using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomStateMachine
{
    public abstract class StateMachine
    {
        private State currentState;

        public State CurrentState { get => currentState; 
            protected set
            {
                currentState?.OnExit();
                currentState = value;
                currentState.OnEnter();
            }
        }

        public void Update()
        {
            State newState = CurrentState.CheckTransitions();

            if (newState != null)
                CurrentState = newState;

            CurrentState.Update();

            UpdateInternal();
        }

        protected abstract void UpdateInternal();
    }
}