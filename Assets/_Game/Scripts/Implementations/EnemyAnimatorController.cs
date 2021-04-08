using System;
using UnityEngine;

namespace WOBH
{
    public class EnemyAnimatorController : AnimatorController
    {

        private static readonly IAnimationState WATCHING = CreateAnimationState("Watching");
        private static readonly IAnimationState DYING = CreateAnimationState("Dying");
        private static readonly IAnimationState WALKING = CreateAnimationState("Walking");

        public event Action OnDieEnds;

        public void Die() => PlayState(DYING);
        public void Watch() => PlayState(WATCHING);
        public void Walk() => PlayState(WALKING);

        private void DieEnds() => OnDieEnds?.Invoke();

    }
}