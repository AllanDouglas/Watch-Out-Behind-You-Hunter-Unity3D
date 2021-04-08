using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace WOBH
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField] Text scoreText;
        [SerializeField] Text bestScoreText;
        [SerializeField] Button restartButton;

        private void Awake()
        {
            GameController.OnGameOver += GameController_OnGameOver;

            restartButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
        }

        private void Start() => gameObject.SetActive(false);


        private void OnDestroy()
        {
            GameController.OnGameOver -= GameController_OnGameOver;
        }

        private void GameController_OnGameOver(object sender, GameStateData state)
        {
            GameController.OnGameOver -= GameController_OnGameOver;
            EventSystem.current.firstSelectedGameObject = restartButton.gameObject;
            gameObject.SetActive(true);
            scoreText.text = state.score.ToString();
            bestScoreText.text = state.bestScore.ToString();
        }
    }
}