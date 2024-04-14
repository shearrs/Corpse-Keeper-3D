using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomStateMachine;

namespace PlayerControl
{
    public abstract class PlayerState : State
    {
        protected Player player;
        protected PlayerMovement playerMovement;
        protected Transform transform;
        protected PlayerFlags flags;
        protected PlayerStats stats;

        public PlayerState(Player player)
        {
            this.player = player;
            this.playerMovement = player.PlayerMovement;
            flags = player.Flags;
            stats = player.Stats;
            transform = player.transform;
        }
    }
}