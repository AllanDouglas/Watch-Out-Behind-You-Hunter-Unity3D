using System;
using UnityEngine;

namespace WOBH
{
    [RequireComponent(typeof(Collider2D))]
    public class Exit : MonoBehaviour
    {
        public static event EventHandler<Collider2D> OnEnterExit;

        [SerializeField] private bool closed;
        private Collider2D mainCollider;

        public void Close()
        {
            closed = true;
            mainCollider.isTrigger = false;
        }

        public void Open()
        {
            closed = false;
            mainCollider.isTrigger = true;
        }

        private void Awake()
        {
            gameObject.layer = LayerMask.NameToLayer("Exit");
            mainCollider = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (closed) return;

            OnEnterExit?.Invoke(this, collision);
        }


    }
}