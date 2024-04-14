namespace PlayerControl
{
    public class PSJump : PlayerState
    {
        public PSJump(Player player) : base(player)
        {
        }

        public override bool UniversalTransition()
        {
            return flags.IsGrounded && PlayerInputHandler.JumpInput;
        }

        protected override void OnEnterInternal()
        {
            stats.JumpBuffer.StopTimer();
            flags.IsJumping = true;
            playerMovement.VerticalMovement = new(0, stats.JumpForce, 0);
        }

        protected override void OnExitInternal()
        {
            flags.IsJumping = false;
        }

        protected override void UpdateInternal()
        {
            if (playerMovement.Velocity.y <= 0)
                flags.IsJumping = false;
        }
    }
}