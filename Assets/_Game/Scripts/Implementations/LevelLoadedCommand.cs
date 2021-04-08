using UnityEngine;

namespace WOBH
{
    public class LevelLoadedCommand : MonoBehaviour
    {
        [SerializeField] AmountDisplayer amountOfEnemiesDisplayer;
        [SerializeField] AmountDisplayer amountOfAmmunitionDisplayer;
        [SerializeField] AmountDisplayer amountOfBulletsDisplayer;
        [SerializeField] AmountDisplayer scoreDisplayer;

        [SerializeField] PlayerController playerController;

        private void Awake()
        {
            LevelController.OnLevelReady += LevelController_OnLevelReady;
            LevelController.OnLevelChanges += LevelController_OnLevelChanges;
            PlayerController.OnPlayerChangeState += PlayerController_OnPlayerChangeState;
            GameController.OnGameStateChange += GameController_OnGameStateChange;
            GameController.OnLevelCompleted += GameController_OnLevelCompleted;
        }



        private void GameController_OnLevelCompleted(object sender, LevelResult e)
        {
            scoreDisplayer.SetText(e.score);
        }

        private void GameController_OnGameStateChange(object sender, GameStateData e)
        {
            scoreDisplayer.SetText(e.score);
        }

        private void PlayerController_OnPlayerChangeState(object sender, PlayerStateData e)
        {
            amountOfAmmunitionDisplayer.SetText(e.ammunition);
            amountOfBulletsDisplayer.SetText(e.bullets);
        }

        private void LevelController_OnLevelChanges(object sender, LevelStateData e)
        {
            amountOfEnemiesDisplayer.SetText(e.totalEnemies - e.deadEnemies);
        }

        private void OnDestroy()
        {
            LevelController.OnLevelChanges -= LevelController_OnLevelChanges;
            LevelController.OnLevelReady -= LevelController_OnLevelReady;
            PlayerController.OnPlayerChangeState -= PlayerController_OnPlayerChangeState;
            GameController.OnGameStateChange -= GameController_OnGameStateChange;
            GameController.OnLevelCompleted -= GameController_OnLevelCompleted;
        }

        private void LevelController_OnLevelReady(object sender, LevelSpawnData e)
        {

            amountOfEnemiesDisplayer.SetText(e.enemies);
            //scoreDisplayer.SetText(e.le);
            amountOfAmmunitionDisplayer.SetText(playerController.Cartridges);
            amountOfBulletsDisplayer.SetText(playerController.Player.Bullets);

        }
    }
}