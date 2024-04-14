using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerControl
{
    public class PlayerFlags
    {
        private readonly CustomController controller;

        public bool IsGrounded => controller.IsGrounded;
        public bool WasGroundedLastFrame => controller.WasGroundedLastFrame;
        public bool IsJumping
        {
            get => isJumping;
            set
            {
                isJumping = value;
                controller.CanBeGrounded = !isJumping;
            }
        }
        public bool GravityEnabled { get; set; } = true;
        private bool isJumping;

        public PlayerFlags(CustomController controller)
        {
            this.controller = controller;
        }
    }
}