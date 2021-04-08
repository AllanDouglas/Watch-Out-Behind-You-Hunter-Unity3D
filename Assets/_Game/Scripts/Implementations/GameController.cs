using System;
using UnityEngine;

namespace WOBH
{

    public class GameController : MonoBehaviour
    {
        public static event EventHandler<LevelResult> OnLevelCompleted;
        public static event EventHandler<LevelRecipie> OnGameReady;
        public static event EventHandler<GameStateData> OnGameStateChange;
        public static event EventHandler<GameStateData> OnGameOver;
        [SerializeField] PlayerController playerController;
        [SerializeField] EnemyController enemyController;
        [Header("Level config")]
        [SerializeField] int pointsEnemyDead = 10;
        [SerializeField] int pointsBonus = 100;

        private SectionService sectionService = DIContainer.Instance.Resolve<SectionService>();
        private LevelService levelService = DIContainer.Instance.Resolve<LevelService>();
        private UserService userService = DIContainer.Instance.Resolve<UserService>();

        private void Awake()
        {
            Application.targetFrameRate = 60;

            LevelController.OnLevelExit += LevelController_OnLevelCompleted;
            LevelController.OnLevelReady += LevelController_OnLevelReady;

            enemyController.OnEnemyDie += EnemyController_OnEnemyDie;
            playerController.OnPlayerDies += PlayerController_OnPlayerDies;
            playerController.OnPlayerSpawns += PlayerController_OnPlayerSpawns;

            sectionService.Level = 1;

        }

        private void EnemyController_OnEnemyDie(Enemy obj)
        {
            sectionService.Points += pointsEnemyDead;
            OnGameStateChange?.Invoke(this, GetGameStateData());
        }

        private void PlayerController_OnPlayerSpawns() => OnGameReady?.Invoke(this, levelService.GetLevelRecipie(sectionService.Level));

        private void LevelController_OnLevelCompleted(object sender, LevelExitData e)
        {
            int levelPass = sectionService.Level;
            sectionService.Level++;
            int bonus = 0;
            if (e.currentLevelState.deadEnemies == e.currentLevelState.totalEnemies)
            {
                playerController.Lives++;
                bonus = pointsBonus * sectionService.Level;
                sectionService.Points += bonus;
            }


            OnLevelCompleted?.Invoke(this, new LevelResult(
                level: levelPass,
                score: sectionService.Points,
                bonus: bonus,
                exit: e.exit,
                totalEnemies: e.currentLevelState.totalEnemies,
                enemiesDead: e.currentLevelState.deadEnemies,
                currentlevel: sectionService.Level,
                nextLevel: levelService.GetLevelRecipie(sectionService.Level))
            );
        }

        private void PlayerController_OnPlayerDies()
        {
            GameStateData gameState = GetGameStateData();
            sectionService.Points = 0;
            sectionService.Level = 1;
            OnGameOver?.Invoke(this, gameState);
        }

        private GameStateData GetGameStateData()
        {
            userService.BestScore = sectionService.Points;
            GameStateData gameState = new GameStateData(sectionService.Points, userService.BestScore);
            return gameState;
        }

        private void OnDestroy()
        {
            LevelController.OnLevelExit -= LevelController_OnLevelCompleted;
            LevelController.OnLevelReady -= LevelController_OnLevelReady;
        }

        private void LevelController_OnLevelReady(object sender, LevelSpawnData e)
        {
            var position = e.entry.transform.position + e.entry.transform.right;

            playerController.SetPlayerPosition(position);
        }

    }
}