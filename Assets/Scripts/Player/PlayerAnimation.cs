using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerControl
{
    public class PlayerAnimation
    {
        private readonly CustomController controller;
        private readonly Animator animator;

        private readonly int isGroundedID = Animator.StringToHash("isGrounded");
        private readonly int moveAmountID = Animator.StringToHash("moveAmount");
        private readonly int isHoldingID = Animator.StringToHash("isHolding");
        private readonly int isRunningID = Animator.StringToHash("isRunning");

        public Animator Animator => animator;

        public PlayerAnimation(CustomController controller, Animator animator)
        {
            this.controller = controller;
            this.animator = animator;
        }

        public void Update()
        {
            animator.SetBool("shears", true);
            animator.SetBool(isGroundedID, controller.IsGrounded);
            animator.SetFloat(moveAmountID, controller.Velocity.magnitude);
            animator.SetBool(isHoldingID, PlayerToolManager.IsHolding);
            animator.SetBool(isRunningID, PlayerInputHandler.SprintInput);
        }
    }
}