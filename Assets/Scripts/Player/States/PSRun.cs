using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

namespace PlayerControl
{
    public class PSRun : PlayerState
    {
        public PSRun(Player player) : base(player)
        {
        }

        public override bool UniversalTransition()
        {
            return PlayerInputHandler.MovementInput.sqrMagnitude > 0.01;
        }

        protected override void OnEnterInternal()
        {
        }

        protected override void OnExitInternal()
        {
        }

        protected override void UpdateInternal()
        {
            HorizontalVelocity();
        }

        private float GetSpeed()
        {
            if (PlayerInputHandler.SprintInput)
                return stats.SprintSpeed;
            else
                return stats.WalkSpeed;
        }

        private void HorizontalVelocity()
        {
            float speed = GetSpeed();

            Vector3 moveAmount = speed * PlayerInputHandler.MovementInput;

            playerMovement.HorizontalMovement = moveAmount;
        }
    }
}