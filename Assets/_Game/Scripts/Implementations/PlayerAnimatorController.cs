using System;

namespace WOBH
{
    public class PlayerAnimatorController : AnimatorController
    {
        private static readonly IAnimationState IDLE = CreateAnimationState("Idle");
        private static readonly IAnimationState WALKING = CreateAnimationState("Walking");
        private static readonly IAnimationState SHOOTING = CreateAnimationState("Shooting");
        private static readonly IAnimationState FIGTHING = CreateAnimationState("Fighting");
        private static readonly IAnimationState ROTATION = CreateAnimationState("Rotating");

        public event Action OnShootCompleted;
        public event Action OnFuckingCompleted;

        public void Idle() => PlayState(IDLE);
        public void Walk() => PlayState(WALKING);
        public void Rotate() => PlayState(ROTATION);
        public void Fight() => PlayState(FIGTHING);
        public void Shoot() => PlayState(SHOOTING);

        private void ShootEnds() => OnShootCompleted?.Invoke();
        private void FuckingEnds() => OnFuckingCompleted?.Invoke();

    }
}