
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace WOBH
{
    public struct LevelSpawnData
    {
        public readonly Level level;
        public readonly Exit entry;
        public readonly int enemies;
        
        public LevelSpawnData(Level level, Exit entry, int enemies)
        {
            this.level = level;
            this.entry = entry;
            this.enemies = enemies;
        }
    }

    public struct LevelExitData
    {
        public readonly Exit exit;
        public LevelStateData currentLevelState;

        public LevelExitData(Exit exit, LevelStateData currentLevelState)
        {
            this.exit = exit;
            this.currentLevelState = currentLevelState;
        }
    }

    public struct LevelStateData
    {
        public readonly int totalEnemies;
        public readonly int deadEnemies;

        public LevelStateData(int totalEnemies, int deadEnemies)
        {
            this.totalEnemies = totalEnemies;
            this.deadEnemies = deadEnemies;
        }
    }

    public class LevelController : MonoBehaviour
    {
        public static event EventHandler<LevelSpawnData> OnLevelReady;
        public static event EventHandler<LevelStateData> OnLevelChanges;
        public static event EventHandler<LevelExitData> OnLevelExit;

        [SerializeField] Level currentLevel;
        [SerializeField] Exit currentEntry;
        [SerializeField] Level[] levelsPrefabs;
        [Header("Controllers")]
        [SerializeField] EnemyController enemiesController;
        [SerializeField] PlayerController playerController;

        private AmmunitionSpawner ammunitionSpawner;
        private StonesSpawner stonesSpawner;
        private BushSpawner bushSpawner;
        private List<Level> levels;
        private List<Ammunition> ammunition;
        private List<EnemySpawnSet> enemiesSet = new List<EnemySpawnSet>(10);
        private List<Enemy> deadEnemies = new List<Enemy>(10);
        private List<Transform> stones;
        private List<Transform> bushes;

        private void Awake()
        {
            Exit.OnEnterExit += Exit_OnEnterExit;
            GameController.OnLevelCompleted += GameController_OnLevelCompleted;
            GameController.OnGameReady += GameController_OnGameReady;
            playerController.OnPlayerRevive += PlayerController_OnPlayerRevive;

            ammunitionSpawner = GetComponent<AmmunitionSpawner>();
            stonesSpawner = GetComponent<StonesSpawner>();
            bushSpawner = GetComponent<BushSpawner>();

            levels = new List<Level>(levelsPrefabs.Length + 1);

            foreach (var prefab in levelsPrefabs)
            {
                var level = Instantiate(prefab);
                level.gameObject.SetActive(false);
                levels.Add(level);
            }

            levels.Add(currentLevel);

        }

        private void GameController_OnGameReady(object sender, LevelRecipie levelRecipie)
        {
            SpawnLevel(currentLevel, currentEntry, levelRecipie);
        }

        private void PlayerController_OnPlayerRevive()
        {
            foreach (var enemySet in enemiesSet)
            {
                if (deadEnemies.Contains(enemySet.enemy)) continue;

                enemySet.enemy.enabled = true;
                enemySet.enemy.gameObject.SetActive(true);
                enemySet.enemy.transform.position = enemySet.startPosition;
            }
        }

        private void OnDestroy()
        {
            Exit.OnEnterExit -= Exit_OnEnterExit;
            GameController.OnLevelCompleted -= GameController_OnLevelCompleted;
            GameController.OnGameReady -= GameController_OnGameReady;
        }

        private void GameController_OnLevelCompleted(object sender, LevelResult levelResult)
        {
            foreach (var level in levels.OrderBy(g => Guid.NewGuid()))
            {

                if (level.HasEntry(levelResult.exit, out var exit))
                {
                    currentLevel.gameObject.SetActive(false);
                    currentLevel = level;
                    level.gameObject.SetActive(true);
                    SpawnLevel(currentLevel, exit, levelResult.nextLevel);
                    break;
                }
            }
        }

        private void SpawnLevel(Level level, Exit exit, LevelRecipie levelRecipie)
        {
            if (ammunition != null)
            {
                foreach (var item in ammunition)
                {
                    ammunitionSpawner.Recycle(item);
                }
            }

            if (stones != null)
            {
                foreach (var stone in stones)
                {
                    stonesSpawner.Recycle(stone);
                }
            }

            if (bushes != null)
            {
                foreach (var bush in bushes)
                {
                    bushSpawner.Recycle(bush);
                }
            }


            stones = stonesSpawner.Spawn(levelRecipie.stones);
            ammunition = ammunitionSpawner.Spawn(levelRecipie.cartridges);
            bushes = bushSpawner.Spawn(levelRecipie.bushes);

            enemiesSet.Clear();
            enemiesController.OnEnemyDie -= Enemy_OnDie;
            
            foreach (var enemy in enemiesController.Spawn(levelRecipie))
            {
                enemiesSet.Add(new EnemySpawnSet(enemy));              
            }

            enemiesController.OnEnemyDie += Enemy_OnDie;

            deadEnemies.Clear();

            level.OpenAllDoors();
            exit.Close();

            OnLevelReady?.Invoke(this, new LevelSpawnData(level,
                        exit, enemiesSet.Count));

            FireLevelStateChange();

        }

        void Enemy_OnDie(Enemy enemy)
        {
            deadEnemies.Add(enemy);
            FireLevelStateChange();
        }

        private void FireLevelStateChange()
        {
            OnLevelChanges?.Invoke(this, new LevelStateData(enemiesSet.Count, deadEnemies.Count));
        }

        private void Exit_OnEnterExit(object sender, Collider2D e)
        {
            var levelStateData = new LevelStateData(enemiesSet.Count, deadEnemies.Count);
            OnLevelExit?.Invoke(this, new LevelExitData(sender as Exit, levelStateData));
        }

        private struct EnemySpawnSet
        {
            public readonly Enemy enemy;
            public readonly Vector2 startPosition;

            public EnemySpawnSet(Enemy enemy)
            {
                this.enemy = enemy;
                this.startPosition = enemy.transform.position;
            }
        }

    }
}