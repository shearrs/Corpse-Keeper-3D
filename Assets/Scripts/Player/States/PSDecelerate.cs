using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerControl
{
    public class PSDecelerate : PlayerState
    {
        private Vector3 initialVelocity;

        private readonly Cooldown decelerationTimer;

        public PSDecelerate(Player player) : base(player)
        {
            decelerationTimer = stats.DecelerationTimer;
        }

        public override bool UniversalTransition()
        {
            return playerMovement.HorizontalMovement.sqrMagnitude > 0.01f && PlayerInputHandler.MovementInput.sqrMagnitude < 0.01f;
        }

        protected override void OnEnterInternal()
        {
            initialVelocity = playerMovement.HorizontalMovement;
            decelerationTimer.StartTimer();
        }

        protected override void OnExitInternal()
        {
            if (!decelerationTimer.Done)
                decelerationTimer.StopTimer();
        }

        protected override void UpdateInternal()
        {
            if (!decelerationTimer.Done)
                Decelerate();
        }

        private void Decelerate()
        {
            playerMovement.HorizontalMovement = Vector3.Lerp(initialVelocity, Vector3.zero, decelerationTimer.Percentage);
        }
    }
}