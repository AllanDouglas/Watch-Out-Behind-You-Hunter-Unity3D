using System.Collections.Generic;
using UnityEngine;

namespace WOBH
{
    public class FoliageSpawner : MonoBehaviour
    {
        [SerializeField] private Transform[] foliages;
        [SerializeField] private Transform[] spawnPoints;

        Spawner<Transform>[] spawners;

        private void Awake()
        {
            spawners = new Spawner<Transform>[foliages.Length];

            for (int i = 0; i < spawners.Length; i++)
            {
                spawners[i] = new Spawner<Transform>(foliages[i]);
            }
        }

        public void Recycle(Transform transform)
        {

        }

        public List<Transform> Spawn(int amount)
        {

            HashSet<Vector2> positions = new HashSet<Vector2>();

            var itens = new List<Transform>(amount);
            for (int i = 0; i < amount; i++)
            {
                int index = Random.Range(0, spawnPoints.Length);
                var position = (Vector2)spawnPoints[index].position;

                if (positions.Contains(position))
                {
                    i--;
                    continue;
                };
                positions.Add(position);
                var spawner = spawners[Random.Range(0, spawners.Length)];
                var item = spawner.Spawn();
                item.position = position;
                itens.Add(item);
            }
            return itens;
        }

        public Transform Spawn(Vector2 position)
        {
            var spawner = spawners[Random.Range(0, spawners.Length)];

            var item = spawner.Spawn();

            item.position = position;

            return item;
        }

    }
}