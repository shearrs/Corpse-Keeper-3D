using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerControl
{
    [CreateAssetMenu(fileName = "New Player Stats", menuName = "Player Stats")]
    public class PlayerStats : ScriptableObject
    {
        [Header("Grounded Movement")]
        [SerializeField] private float maxSpeed = 20;
        [SerializeField] private float walkSpeed = 3;
        [SerializeField] private float sprintSpeed = 8;

        [Header("Aerial Movement")]
        [SerializeField] private float airSpeed = 30;
        [SerializeField] private float fallSpeed = 1.5f;
        [SerializeField] private float jumpForce = 4;

        [Header("Cooldowns")]
        [SerializeField] private Cooldown decelerationTimer;
        [SerializeField] private Cooldown jumpBuffer;

        public float MaxSpeed => maxSpeed;
        public float WalkSpeed => walkSpeed;
        public float SprintSpeed => sprintSpeed;
        public float AirSpeed => airSpeed;
        public float FallSpeed => fallSpeed;
        public float JumpForce => jumpForce;

        public Cooldown DecelerationTimer => decelerationTimer;
        public Cooldown JumpBuffer => jumpBuffer;
    }
}