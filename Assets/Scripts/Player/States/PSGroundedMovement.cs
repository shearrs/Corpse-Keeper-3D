using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomStateMachine;

namespace PlayerControl
{
    public class PSGroundedMovement : PlayerState
    {
        public PSGroundedMovement(Player player) : base(player)
        {
            PSDecelerate decelerateState = new(player);
            PSRun runState = new(player);
            PSIdle idleState = new(player);

            runState.Transitions = new Transition[] { new(decelerateState) };
            idleState.Transitions = new Transition[] { new(runState) };
            decelerateState.Transitions = new Transition[] { new(idleState), new(runState) };

            defaultSubState = idleState;
        }

        public override bool UniversalTransition()
        {
            return flags.IsGrounded && !flags.IsJumping;
        }

        protected override void OnEnterInternal()
        {
        }

        protected override void OnExitInternal()
        {
        }

        protected override void UpdateInternal()
        {
        }
    }
}