using System;
using System.Collections.Generic;
using UnityEngine;

namespace WOBH
{
    [RequireComponent(typeof(EnemySpawner))]
    [RequireComponent(typeof(FoliageSpawner))]
    [RequireComponent(typeof(EnemySoundController))]
    public class EnemyController : MonoBehaviour
    {
        private EnemySpawner enemySpawner;
        private FoliageSpawner foliageSpawner;
        private EnemySoundController enemySoundController;
        private List<Transform> foliages = new List<Transform>();
        private HashSet<EnemyHandler> enemyHandlers = new HashSet<EnemyHandler>();
        public event Action<Enemy> OnEnemyDie;

        private void Awake()
        {
            enemySpawner = GetComponent<EnemySpawner>();
            foliageSpawner = GetComponent<FoliageSpawner>();
            enemySoundController = GetComponent<EnemySoundController>();
        }

        public List<Enemy> Spawn(LevelRecipie levelRecipie)
        {
            foreach (var enemyHandler in enemyHandlers)
            {
                enemyHandler.Recycle();
            }

            if (foliages != null)
                foreach (var foliage in foliages)
                {
                    Destroy(foliage.gameObject);
                }

            foliages.Clear();

            var enemies = enemySpawner.Spawn(levelRecipie.enemies);
            enemyHandlers.Clear();

            foliages.AddRange(foliageSpawner.Spawn(levelRecipie.foliages));


            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy enemy = enemies[i];
                enemyHandlers.Add(new EnemyHandler(enemy, this));
                enemy.transform.position = foliages[i].position;
            }


            return enemies;
        }

        private void Enemy_OnDie(Enemy enemy)
        {
            enemySoundController.Scream();
            OnEnemyDie?.Invoke(enemy);
        }

        void Enemy_OnBite()
        {
            foreach (var handle in enemyHandlers)
            {
                handle.enemy.Watch();
            }
        }

        private class EnemyHandler
        {
            public readonly Enemy enemy;
            private readonly EnemyController controller;

            public EnemyHandler(Enemy enemy, EnemyController controller)
            {
                this.enemy = enemy;
                this.controller = controller;

                this.enemy.OnBite += OnBite;
                this.enemy.OnDie += OnDie;
            }

            private void OnBite() => this.controller.Enemy_OnBite();

            private void OnDie()
            {
                this.enemy.OnDie -= OnDie;
                this.controller.Enemy_OnDie(enemy);
            }

            public void Recycle()
            {
                this.enemy.OnBite -= OnBite;
                this.enemy.OnDie -= OnDie;
                controller.enemySpawner.Recycle(enemy);
                enemy.Stop();
            }
        }
    }

}