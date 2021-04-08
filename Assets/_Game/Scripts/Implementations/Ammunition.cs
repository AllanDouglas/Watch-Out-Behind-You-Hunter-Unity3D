using System;
using UnityEngine;

namespace WOBH
{
    public class Ammunition : MonoBehaviour
    {
        public static event EventHandler<int> OnCollision;

        [SerializeField] private int payload = 5;

        public int Payload { get => payload; }

        public void Catch() => gameObject.SetActive(false);

        private void OnTriggerEnter2D(Collider2D collision)
        {
            OnCollision?.Invoke(this, payload);
        }
    }
}