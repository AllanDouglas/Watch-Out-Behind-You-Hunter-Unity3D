using System;
using UnityEngine;

namespace WOBH
{
    public class LivesDisplayer : MonoBehaviour
    {
        [SerializeField] GameObject[] lives;

        private void Awake()
        {
            PlayerController.OnPlayerChangeState += PlayerController_OnPlayerChangeState;

            ShowLives(0);

        }

        private void OnDestroy()
        {
            PlayerController.OnPlayerChangeState -= PlayerController_OnPlayerChangeState;
        }

        private void ShowLives(int total)
        {
            for (int i = 0; i < lives.Length; i++)
            {
                if (i + 1 > total)
                {
                    lives[i].SetActive(false);
                    continue;
                }

                lives[i].SetActive(true);
            }
        }

        private void PlayerController_OnPlayerChangeState(object sender, PlayerStateData e)
        {
            ShowLives(e.lives);
        }
    }
}