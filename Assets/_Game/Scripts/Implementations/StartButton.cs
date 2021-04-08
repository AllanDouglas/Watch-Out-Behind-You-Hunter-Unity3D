using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace WOBH
{
    [RequireComponent(typeof(Button))]
    public class StartButton : MonoBehaviour
    {
        [SerializeField] int indexSceneDebug = 1;

        private void Awake()
        {
            var button = GetComponent<Button>();

            button.onClick.AddListener(() =>
            {

#if UNITY_ANDROID && !UNITY_EDITOR
                SceneManager.LoadScene(2);
#elif !UNITY_EDITOR
                SceneManager.LoadScene(1);
#else
                SceneManager.LoadScene(indexSceneDebug);
#endif


            });
        }

    }
}