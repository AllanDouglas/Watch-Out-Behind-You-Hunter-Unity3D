
using System;
using UnityEngine;

namespace WOBH
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerAnimatorController))]
    public class Player : MonoBehaviour
    {
        private const int MAX_BULLETS = 5;

        public event Action OnDie;
        public event Action OnCaptured;

        [SerializeField] private float movementSpeed = 10;
        [SerializeField] private float rotationSpeed = 5;

        [SerializeField] private Collider2D hitAreaCollider;
        [SerializeField] private LayerMask enemyLayerMask;

        private PlayerAnimatorController playerAnimatorController;
        private Rigidbody2D rb;

        private float moveDirection;

        private int bullets = MAX_BULLETS;
        private bool shouldRotate;
        private float targetAngle;

        private Collider2D[] hitColliders = new Collider2D[9];

        public int Bullets => bullets;

        public bool IsAlive { get; private set; } = true;
        public float MovementSpeed => movementSpeed;
        public float RotationSpeed => rotationSpeed;

        public float Rotation => rb.rotation;

        public void Reload() => bullets = MAX_BULLETS;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            playerAnimatorController = GetComponent<PlayerAnimatorController>();

            playerAnimatorController.OnShootCompleted += OnShootCompleted;
            playerAnimatorController.OnFuckingCompleted += PlayerAnimatorController_OnFuckingCompleted;

            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        private void PlayerAnimatorController_OnFuckingCompleted()
        {
            OnDie?.Invoke();
        }

        private void OnShootCompleted()
        {
            enabled = true;
        }

        private void LateUpdate()
        {

            if (moveDirection != 0)
            {
                playerAnimatorController.Walk();
                return;
            }


            if (shouldRotate)
            {
                playerAnimatorController.Rotate();
                return;
            }

            playerAnimatorController.Idle();

        }

        public void Shoot()
        {
            bullets--;
            enabled = false;

            var filter = new ContactFilter2D() {
                layerMask = enemyLayerMask,
                useLayerMask = true
            };

            int enemies = Physics2D.OverlapCollider(hitAreaCollider, filter, hitColliders);

            for (int i = 0; i < enemies; i++)
            {
                if (hitColliders[i].TryGetComponent<Enemy>(out var enemy))
                {
                    enemy.Hit();
                }
            }

            playerAnimatorController.Shoot();
        }

        private void FixedUpdate()
        {
            float fixedDeltaTime = Time.fixedDeltaTime;
            if (moveDirection != 0)
            {

                rb.MovePosition(
                        Vector2.MoveTowards(rb.position,
                                            rb.position + (Vector2)(transform.right * moveDirection),
                                            movementSpeed * fixedDeltaTime));
                moveDirection = 0;
            }

            if (shouldRotate)
            {
                shouldRotate = false;
                rb.rotation = Mathf.LerpAngle(rb.rotation, targetAngle, rotationSpeed * fixedDeltaTime);
            }
        }

        internal void Revive()
        {
            IsAlive = true;
            rb.isKinematic = false;
            rb.bodyType = RigidbodyType2D.Dynamic;
            enabled = true;
        }

        internal void Rotate(float angle)
        {
            shouldRotate = true;
            targetAngle = angle;
        }

        internal void Move(float movement) => moveDirection = movement;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.collider.CompareTag("Enemy")) return;

            collision.collider.gameObject.SetActive(false);
            Die();
        }

        private void Die()
        {
            IsAlive = false;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
            rb.isKinematic = true;
            rb.bodyType = RigidbodyType2D.Kinematic;
            enabled = false;
            playerAnimatorController.Fight();
            OnCaptured?.Invoke();
        }
    }
}