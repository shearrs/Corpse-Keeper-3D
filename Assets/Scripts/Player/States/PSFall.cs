using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerControl
{
    public class PSFall : PlayerState
    {
        public PSFall(Player player) : base(player)
        {
        }

        protected override void OnEnterInternal()
        {
            flags.GravityEnabled = true;
        }

        protected override void OnExitInternal()
        {
        }

        protected override void UpdateInternal()
        {
        }
    }
}