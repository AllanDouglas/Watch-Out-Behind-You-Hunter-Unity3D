using UnityEngine;

namespace WOBH
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] GameObject controllerTips;

        private void Awake()
        {
#if UNITY_ANDROID || UNITY_IOS
            controllerTips.SetActive(false);
#endif
        }

    }
}