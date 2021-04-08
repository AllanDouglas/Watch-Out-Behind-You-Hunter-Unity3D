using System;
using UnityEngine;

namespace WOBH
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorController : MonoBehaviour
    {
        protected Animator animator;
        private IAnimationState lastState;

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void PlayState(IAnimationState state)
        {
            if (lastState?.Equals(state) ?? false) return;
            lastState = state;
            animator?.Play(state.Name);
        }

        public static IAnimationState CreateAnimationState(string name)
        {
            return new AnimationState(name);
        }

        private struct AnimationState : IAnimationState, IEquatable<AnimationState>
        {
            public readonly string name;

            public AnimationState(string name)
            {
                this.name = name;
            }

            public string Name => name;

            public bool Equals(AnimationState other) => name == other.name;
        }
    }

    public interface IAnimationState
    {
        string Name { get; }
    }
}