using System;
using UnityEngine;
namespace WOBH
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(EnemyAnimatorController))]
    public class Enemy : MonoBehaviour
    {
        public event Action OnDie;
        public event Action OnBite;

        [SerializeField] private float viewField = 5;
        [SerializeField] private LayerMask viewMask;
        [Header("Movement")]
        [SerializeField] private float movementSpeed = 10;
        private Rigidbody2D rb;
        private EnemyAnimatorController animator;
        private Collider2D mainCollider;
        private Collider2D[] results = new Collider2D[1];
        private bool targetFounded;
        private Transform target;

        public void Watch()
        {
            if (target == null) return;
            Stop();
            animator.Watch();
        }

        private void OnEnable()
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            mainCollider.enabled = true;
            target = null;
            animator.Walk();
        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<EnemyAnimatorController>();
            mainCollider = GetComponent<Collider2D>();

            animator.OnDieEnds += () => gameObject.SetActive(false);
        }

        private void Update() => CheckTarget();

        private void CheckTarget()
        {
            if (Time.frameCount % 10 != 0) return;
            var count = Physics2D.OverlapCircleNonAlloc(transform.position, viewField, results, viewMask);
            if (count == 0) return;
            targetFounded = true;
            target = results[0].transform;
        }

        private void FixedUpdate()
        {
            if (targetFounded == false) return;
            Rotate();
            Move();
        }

        private void Move()
        {
            rb.MovePosition(
                Vector2.MoveTowards(rb.position, rb.position + (Vector2)(transform.up), movementSpeed * Time.fixedDeltaTime)
            );
        }

        private void Rotate()
        {
            Vector3 dir = (target.position - transform.position).normalized;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            rb.MoveRotation(Quaternion.AngleAxis(angle - 90, Vector3.forward));
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player") == false) return;

            Bite();
        }

        private void Bite()
        {
            mainCollider.enabled = false;
            OnBite?.Invoke();
        }

        public void Hit()
        {
            Die();
        }

        private void Die()
        {
            if (mainCollider.enabled == false) return;

            Stop();
            targetFounded = false;
            target = null;
            mainCollider.enabled = false;
            animator.Die();
            OnDie?.Invoke();

        }

        public void Stop()
        {
            target = null;
            targetFounded = false;
            enabled = false;
            rb.isKinematic = true;
            rb.angularVelocity = 0;
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, viewField);
        }
#endif

    }
}