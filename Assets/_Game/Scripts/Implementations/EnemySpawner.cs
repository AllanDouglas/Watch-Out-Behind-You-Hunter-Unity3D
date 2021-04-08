using System.Collections.Generic;
using UnityEngine;

namespace WOBH
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] Enemy enemyPrefab;
        [SerializeField] Transform[] spawnPoints;

        Spawner<Enemy> spawner;
        
        private void Awake()
        {
            spawner = new Spawner<Enemy>(enemyPrefab);
        }

        public void Recycle(Enemy enemy)
        {
            enemy.gameObject.SetActive(false);
            spawner.Recycle(enemy);
        }

        public List<Enemy> Spawn(int amount)
        {
            var enemies = new List<Enemy>(amount);

            for (int i = 0; i < amount; i++)
            {
                Enemy item = spawner.Spawn();

                item.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                item.gameObject.SetActive(true);
                item.enabled = true;
                enemies.Add(item);
            }

            return enemies;
        }
    }

}