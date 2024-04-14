using System.Collections;
using System.Collections.Generic;
using Tweens;
using UnityEngine;

namespace PlayerControl
{
    public class PlayerMovement
    {
        private readonly CustomController controller;

        public Vector3 HorizontalMovement
        {
            get => new(Velocity.x, 0, Velocity.z);
            set
            {
                Vector3 movement = new(value.x, Velocity.y, value.z);
                Velocity = movement;
            }
        }
        public Vector3 VerticalMovement
        {
            get => new(0, Velocity.y, 0);
            set
            {
                Vector3 movement = new(Velocity.x, value.y, Velocity.z);
                Velocity = movement;
            }
        }
        public Vector3 Velocity { get; set; }

        public PlayerMovement(CustomController controller)
        {
            this.controller = controller;
        }

        public void Update()
        {
            ApplyVelocity();
        }

        private void ApplyVelocity()
        {
            Vector3 displacement = Velocity * Time.deltaTime;

            controller.Move(displacement);

            Velocity = controller.Velocity;
        }
    }
}