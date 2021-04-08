using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WOBH
{
    public class LevelCompletedScreen : MonoBehaviour
    {
        [SerializeField] Text level;
        [SerializeField] Text bonus;
        [SerializeField] Text extraLife;
        [SerializeField] Text cartridges;
        [SerializeField] Button continueButton;

        [SerializeField] InputControllerAdapter inputController;

        private void Awake()
        {
            GameController.OnLevelCompleted += GameController_OnLevelCompleted;
            continueButton.onClick.AddListener(Close);
        }

        private void Start() => Close();

        private void OnDestroy()
        {
            GameController.OnLevelCompleted -= GameController_OnLevelCompleted;
        }

        private void GameController_OnLevelCompleted(object sender, LevelResult levelResult)
        {
            Fill(levelResult);
            Open();
        }

        private void Open()
        {
            EventSystem.current.firstSelectedGameObject = continueButton.gameObject;
            Time.timeScale = 0;
            inputController.enabled = false;
            gameObject.SetActive(true);
        }

        private void Fill(LevelResult levelResult)
        {
            level.text = (levelResult.level + 1).ToString();
            bonus.text = levelResult.bonus.ToString();
            extraLife.gameObject.SetActive(levelResult.bonus > 0);
            cartridges.text = $"THERE ARE {levelResult.nextLevel.cartridges} CARTRIDGES IN THIS LEVEL!";
        }

        private void Close()
        {
            Time.timeScale = 1;
            inputController.enabled = true;
            gameObject.SetActive(false);
        }
    }
}