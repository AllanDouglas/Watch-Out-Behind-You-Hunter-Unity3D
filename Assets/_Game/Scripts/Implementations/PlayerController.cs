using System;
using UnityEngine;

namespace WOBH
{
    public struct PlayerStateData
    {
        public readonly int ammunition;
        public readonly int bullets;
        public readonly int lives;

        public PlayerStateData(int ammunition, int bullets, int lives)
        {
            this.ammunition = ammunition;
            this.bullets = bullets;
            this.lives = lives;
        }
    }

    public class PlayerController : MonoBehaviour
    {
        private const int MAX_HITS = 5;
        public static event EventHandler<PlayerStateData> OnPlayerChangeState;

        [SerializeField] private Player playerPrefab;
        [SerializeField] private Transform leftSpawnPoint;
        [SerializeField] private int startLives = 1;
        [SerializeField] private InputControllerAdapter inputController;

        private Vector3 lastPosition;
        public Player Player { get; private set; }
        public int Cartridges => cartridges;

        public int Lives
        {
            get => lives;
            internal set
            {
                lives = Mathf.Clamp(value, 0, MAX_HITS);
                FireChangeState();
            }
        }

        private int cartridges;

        private SoundPlayerController soundPlayerController;
        private int lives;
        private float currentAngle = 999;
        private Movement lastMovement;

        public event Action OnPlayerSpawns;
        public event Action OnPlayerDies;
        public event Action OnPlayerRevive;
        public void SetPlayerPosition(Vector3 position)
        {
            if (Player == null)
            {
                SpawnPlayer();
            }

            Player.transform.position = position;
        }

        private void Awake()
        {
            Ammunition.OnCollision += Ammunition_OnCollision;
            lives = startLives;
            soundPlayerController = GetComponent<SoundPlayerController>();
            lastPosition = leftSpawnPoint.position;

            InputEvents();
        }

        private void InputEvents()
        {
            inputController.OnFire += InputController_OnFire;
            inputController.OnReload += InputController_OnReload;
            inputController.OnMovement += InputController_OnMovement;
        }

        private void InputController_OnMovement(Movement movement)
        {
            if (Player.IsAlive == false) return;

            if (Mathf.Approximately(movement.movementDirection, 0) == false)
                PlayerMove(movement.movementDirection);

            if (movement.rotation != lastMovement.rotation)
                PlayerRotate(movement.rotation);

            lastMovement = movement;

        }

        private void InputController_OnReload()
        {
            if (Player.IsAlive != false && cartridges > 0)
            {
                Reload();
            }
        }

        private void InputController_OnFire()
        {
            if (Player.IsAlive == false) return;

            if (Player.Bullets > 0)
            {
                Shoot();
            }
            else
            {
                soundPlayerController.NoBullets();
            }
        }

        private void PlayerMove(float direction) => Player.Move(direction);
        private void PlayerRotate(float angle) => Player.Rotate(angle);

        private void OnDestroy()
        {
            Ammunition.OnCollision -= Ammunition_OnCollision;
        }

        private void Ammunition_OnCollision(object sender, int amount)
        {
            (sender as Ammunition).gameObject.SetActive(false);
            cartridges++;
            soundPlayerController.GetAmmunition();
            FireChangeState();
        }

        private void Start()
        {
            if (Player == null) SpawnPlayer();
        }

        private void SpawnPlayer()
        {
            Player = Instantiate(playerPrefab);
            Player.transform.position = lastPosition;
            Player.OnDie += Player_OnDie;
            Player.OnCaptured += Player_OnCaptured;
            OnPlayerSpawns?.Invoke();
            FireChangeState();
        }

        private void Player_OnCaptured() => soundPlayerController.Scream();

        private void Player_OnDie()
        {
            lastPosition = Player.transform.position;

            lives--;

            if (lives > 0)
            {
                Player.Revive();
                OnPlayerRevive?.Invoke();
                FireChangeState();
                return;
            }

            OnPlayerDies?.Invoke();
        }

        private void Shoot()
        {
            Player.Shoot();
            soundPlayerController.Shoot();
            FireChangeState();
        }

        private void Reload()
        {
            cartridges--;
            Player.Reload();
            soundPlayerController.Reload();
            FireChangeState();
        }

        private void FireChangeState()
        {
            OnPlayerChangeState?.Invoke(this,
                new PlayerStateData(cartridges, Player.Bullets, lives));
        }
    }
}