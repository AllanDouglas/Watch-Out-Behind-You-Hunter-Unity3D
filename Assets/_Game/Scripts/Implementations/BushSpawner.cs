using System;
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
                var bush = spawner.Spawn();
                bush.transform.position = poinst[i].position;
                bush.gameObject.SetActive(true);
                list.Add(bush);
            }

            return list;
        }
        internal void Recycle(Transform bursh)
        {
            bursh.gameObject.SetActive(false);
            spawner.Recycle(bursh);
        }

        private void Awake()
        {
            spawner = new Spawner<Transform>(prefab);
        }

        
    }
}