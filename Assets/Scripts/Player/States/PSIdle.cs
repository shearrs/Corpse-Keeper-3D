using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerControl
{
    public class PSIdle : PlayerState
    {
        public PSIdle(Player player) : base(player)
        {
        }

        public override bool UniversalTransition()
        {
            return playerMovement.HorizontalMovement.sqrMagnitude < 0.01f;
        }

        protected override void OnEnterInternal()
        {
            playerMovement.HorizontalMovement = Vector3.zero;
        }

        protected override void OnExitInternal()
        {
        }

        protected override void UpdateInternal()
        {
        }
    }
}