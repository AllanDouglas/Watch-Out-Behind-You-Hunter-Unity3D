using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WOBH
{
    public class BushSpawner : MonoBehaviour
    {
        [SerializeField] Transform prefab;

        [SerializeField] Transform[] spawnPoints;

        Spawner<Transform> spawner;

        public List<Transform> Spawn(int amount)
        {
            var list = new List<Transform>();

            var poinst = spawnPoints.OrderBy(d => Guid.NewGuid()).ToArray();

            for (int i = 0; i < amount; i++)
            {
                var stone = spawner.Spawn();
                stone.transform.position = poinst[i].position;
                list.Add(stone);
            }

            return list;
        }
        internal void Recycle(Transform stone) => spawner.Recycle(stone);

        private void Awake()
        {
            spawner = new Spawner<Transform>(prefab);
        }

        
    }
}