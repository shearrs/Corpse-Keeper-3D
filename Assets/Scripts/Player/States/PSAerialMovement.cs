using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomStateMachine;

namespace PlayerControl
{
    public class PSAerialMovement : PlayerState
    {
        public PSJump JumpState => jumpState;

        private float initialHorizontalSpeed;

        private readonly PSJump jumpState;

        public PSAerialMovement(Player player) : base(player)
        {
            PSFall fallState = new(player);
            jumpState = new(player);

            jumpState.Transitions = new Transition[] { 
                new(fallState, () => fallState.UniversalTransition() && !flags.IsJumping), 
            };

            defaultSubState = fallState;
        }

        public override bool UniversalTransition()
        {
            return !flags.IsGrounded;
        }

        protected override State InitializeSubState()
        {
            if (jumpState.UniversalTransition())
                return jumpState;

            return base.InitializeSubState();
        }

        protected override void OnEnterInternal()
        {
            initialHorizontalSpeed = playerMovement.HorizontalMovement.magnitude;
            if (initialHorizontalSpeed < 1)
                initialHorizontalSpeed = 1;
        }

        protected override void OnExitInternal()
        {
        }

        protected override void UpdateInternal()
        {
            HorizontalMovement();

            if (flags.GravityEnabled)
                Gravity();
        }

        private void HorizontalMovement()
        {
            Vector3 newMovement = PlayerInputHandler.MovementInput * stats.AirSpeed;
            Vector3 velocity = playerMovement.HorizontalMovement;

            // remap the velocity to be a bit in the new direction but keep the speed the same
            velocity += newMovement * Time.deltaTime;

            if (velocity.magnitude > initialHorizontalSpeed)
                velocity = velocity.normalized * initialHorizontalSpeed;

            playerMovement.HorizontalMovement = velocity;
        }

        private void Gravity()
        {
            Vector3 newVelocity = playerMovement.Velocity;

            if (newVelocity.y < -0.01)
            {
                newVelocity.y += Physics.gravity.y * stats.FallSpeed * Time.deltaTime;
            }
            else
                newVelocity.y += Physics.gravity.y * Time.deltaTime;

            playerMovement.Velocity = newVelocity;
        }
    }
}