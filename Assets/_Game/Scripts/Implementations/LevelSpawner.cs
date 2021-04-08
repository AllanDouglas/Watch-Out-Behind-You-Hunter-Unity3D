using System.Collections;
using UnityEngine;

namespace WOBH
{
    public class LevelSpawner : MonoBehaviour
    {
        [SerializeField] Level[] levels;

        Spawner<Level>[] spawner;

        private void Awake()
        {
            spawner = new Spawner<Level>[levels.Length];

            for (int i = 0; i < spawner.Length; i++)
            {
                spawner[i] = new Spawner<Level>(levels[i]);
            }

        }
    }
}