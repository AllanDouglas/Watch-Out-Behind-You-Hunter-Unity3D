using System;
using UnityEngine;

namespace WOBH
{
    public abstract class InputControllerAdapter : MonoBehaviour
    {
        public event Action OnFire;
        public event Action OnReload;
        public event Action<Vector2> OnAxis;
        public event Action<Movement> OnMovement;

        public virtual bool Enabled { get; set; }
        protected virtual void DispatchOnFire() => OnFire?.Invoke();
        protected virtual void DispatchOnReload() => OnReload?.Invoke();
        protected virtual void DispatchOnAxis(Vector2 movement) => OnAxis?.Invoke(movement);
        protected virtual void DispatchOnMovement(Movement movement) => OnMovement?.Invoke(movement);
    }

    public struct Movement
    {
        public readonly float movementDirection;
        public readonly float rotation;

        public Movement(float direction, float rotation)
        {
            this.movementDirection = Mathf.Clamp(direction, -1, 1);
            this.rotation = rotation;
        }
    }
}