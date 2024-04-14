using CustomStateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerControl
{
    [System.Serializable]
    public class PlayerStateMachine : StateMachine
    {
        [SerializeField] private List<string> displayStates = new();
        private readonly PlayerFlags playerFlags;

        public PlayerFlags Flags => playerFlags;

        public PlayerStateMachine(Player player)
        {
            PSGroundedMovement groundedState = new(player);
            PSAerialMovement aerialState = new(player);

            groundedState.Transitions = new Transition[] { new(aerialState), new(aerialState, () => aerialState.JumpState.UniversalTransition()) };
            aerialState.Transitions = new Transition[] { new(groundedState) };

            CurrentState = groundedState;
        }

        protected override void UpdateInternal()
        {
            displayStates.Clear();

            State state = CurrentState;

            while (state != null)
            {
                displayStates.Add(state.ToString());
                state = state.SubState;
            }
        }
    }
}