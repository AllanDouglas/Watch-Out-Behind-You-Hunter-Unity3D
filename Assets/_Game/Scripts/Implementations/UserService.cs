using UnityEngine;

namespace WOBH
{
    public class UserService
    {
        private const string Key = "BEST_SCORE";

        public int BestScore
        {
            get => PlayerPrefs.GetInt(Key, 0);
            set => PlayerPrefs.SetInt(Key, Mathf.Max(value, BestScore));
        }

    }
}